using AutoMapper;
using FastServer.Application.DTOs;
using FastServer.Application.EventPublishers;
using FastServer.Application.Events.LogServicesContentEvents;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Services;

/// <summary>
/// Servicio de aplicación para gestionar contenidos de logs (LogServicesContent).
/// Proporciona operaciones CRUD y consultas especializadas usando PostgreSQL (BD: FastServer_Logs).
/// </summary>
public class LogServicesContentService : ILogServicesContentService
{
    private readonly ILogsDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogServicesContentEventPublisher _eventPublisher;

    /// <summary>
    /// Inicializa una nueva instancia del servicio LogServicesContentService.
    /// </summary>
    /// <param name="context">Contexto de base de datos PostgreSQL para logs (FastServer_Logs)</param>
    /// <param name="mapper">Mapeador para convertir entre entidades y DTOs</param>
    /// <param name="eventPublisher">Publisher de eventos para suscripciones GraphQL</param>
    public LogServicesContentService(
        ILogsDbContext context,
        IMapper mapper,
        ILogServicesContentEventPublisher eventPublisher)
    {
        _context = context;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<PaginatedResultDto<LogServicesContentDto>> GetAllAsync(
        PaginationParamsDto pagination,
        CancellationToken cancellationToken = default)
    {
        int totalCount = await _context.LogServicesContents.CountAsync(cancellationToken);

        List<LogServicesContent> entities = await _context.LogServicesContents
            .AsNoTracking()
            .OrderByDescending(x => x.LogServicesDate)
            .Skip(pagination.Skip)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResultDto<LogServicesContentDto>
        {
            Items = _mapper.Map<IEnumerable<LogServicesContentDto>>(entities),
            TotalCount = totalCount,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize
        };
    }

    public async Task<LogServicesContentDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        LogServicesContent? entity = await _context.LogServicesContents
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.LogId == id, cancellationToken);

        return entity == null ? null : _mapper.Map<LogServicesContentDto>(entity);
    }

    public async Task<IEnumerable<LogServicesContentDto>> GetByLogIdAsync(long logId, CancellationToken cancellationToken = default)
    {
        List<LogServicesContent> entities = await _context.LogServicesContents
            .AsNoTracking()
            .Where(x => x.LogId == logId)
            .OrderBy(x => x.LogServicesDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<LogServicesContentDto>>(entities);
    }

    public async Task<IEnumerable<LogServicesContentDto>> SearchByContentAsync(string searchText, CancellationToken cancellationToken = default)
    {
        List<LogServicesContent> entities = await _context.LogServicesContents
            .AsNoTracking()
            .Where(x => x.LogServicesContentText != null && x.LogServicesContentText.Contains(searchText))
            .OrderByDescending(x => x.LogServicesDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<LogServicesContentDto>>(entities);
    }

    public async Task<LogServicesContentDto> CreateAsync(CreateLogServicesContentDto dto, CancellationToken cancellationToken = default)
    {
        LogServicesContent entity = _mapper.Map<LogServicesContent>(dto);
        entity.LogServicesContentId = Guid.CreateVersion7();

        await _context.LogServicesContents.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        LogServicesContentDto result = _mapper.Map<LogServicesContentDto>(entity);

        await _eventPublisher.PublishLogServicesContentCreatedAsync(new LogServicesContentCreatedEvent
        {
            LogServicesContentId = result.LogServicesContentId,
            LogId = result.LogId,
            EventName = result.EventName,
            LogServicesDate = result.LogServicesDate,
            LogServicesLogLevel = result.LogServicesLogLevel,
            LogServicesState = result.LogServicesState,
            LogServicesContentText = result.LogServicesContentText,
            CreatedAt = DateTime.UtcNow
        });

        return result;
    }

    public async Task<BulkInsertResultDto<LogServicesContentDto>> CreateBulkAsync(
        IEnumerable<CreateLogServicesContentDto> dtos,
        CancellationToken cancellationToken = default)
    {
        List<CreateLogServicesContentDto> dtoList = dtos.ToList();

        if (dtoList.Count == 0)
        {
            return new BulkInsertResultDto<LogServicesContentDto>
            {
                TotalRequested = 0,
                TotalInserted = 0,
                Success = true
            };
        }

        if (dtoList.Count > 1000)
        {
            return new BulkInsertResultDto<LogServicesContentDto>
            {
                TotalRequested = dtoList.Count,
                TotalInserted = 0,
                Success = false,
                ErrorMessage = "El límite máximo por lote es de 1000 registros."
            };
        }

        var validDtos = new List<CreateLogServicesContentDto>();
        var errors = new List<BulkInsertError>();

        for (int i = 0; i < dtoList.Count; i++)
        {
            CreateLogServicesContentDto dto = dtoList[i];
            string? validationError = ValidateLogServicesContent(dto);
            if (validationError != null)
            {
                errors.Add(new BulkInsertError
                {
                    Index = i,
                    ErrorMessage = validationError,
                    FailedItem = dto
                });
            }
            else
            {
                validDtos.Add(dto);
            }
        }

        if (validDtos.Count == 0)
        {
            return new BulkInsertResultDto<LogServicesContentDto>
            {
                TotalRequested = dtoList.Count,
                TotalInserted = 0,
                TotalFailed = errors.Count,
                Success = false,
                ErrorMessage = "Ningún registro pasó la validación.",
                Errors = errors
            };
        }

        List<LogServicesContent> entities = _mapper.Map<List<LogServicesContent>>(validDtos);

        // Generar LogServicesContentId para cada entidad
        foreach (LogServicesContent entity in entities)
        {
            entity.LogServicesContentId = Guid.CreateVersion7();
        }

        var strategy = _context.Database.CreateExecutionStrategy();

        try
        {
            List<LogServicesContentDto> results = await strategy.ExecuteAsync(async ct =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync(ct);
                await _context.LogServicesContents.AddRangeAsync(entities, ct);
                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
                return _mapper.Map<List<LogServicesContentDto>>(entities);
            }, cancellationToken);

            // Publicar eventos en background (fire-and-forget)
            _ = Task.Run(async () =>
            {
                foreach (LogServicesContentDto result in results)
                {
                    try
                    {
                        await _eventPublisher.PublishLogServicesContentCreatedAsync(new LogServicesContentCreatedEvent
                        {
                            LogServicesContentId = result.LogServicesContentId,
                            LogId = result.LogId,
                            EventName = result.EventName,
                            LogServicesDate = result.LogServicesDate,
                            LogServicesLogLevel = result.LogServicesLogLevel,
                            LogServicesState = result.LogServicesState,
                            LogServicesContentText = result.LogServicesContentText,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                    catch { /* Log silencioso - no bloquea respuesta */ }
                }
            }, CancellationToken.None);

            return new BulkInsertResultDto<LogServicesContentDto>
            {
                InsertedItems = results,
                TotalRequested = dtoList.Count,
                TotalInserted = results.Count,
                TotalFailed = errors.Count,
                Success = true,
                Errors = errors
            };
        }
        catch (Exception ex)
        {
            return new BulkInsertResultDto<LogServicesContentDto>
            {
                TotalRequested = dtoList.Count,
                TotalInserted = 0,
                TotalFailed = dtoList.Count,
                Success = false,
                ErrorMessage = ex.Message,
                Errors = errors
            };
        }
    }

    private static string? ValidateLogServicesContent(CreateLogServicesContentDto dto)
    {
        if (dto.LogId <= 0)
            return "LogId debe ser mayor a 0.";
        if (string.IsNullOrWhiteSpace(dto.EventName))
            return "EventName es requerido.";
        return null;
    }

    public async Task<LogServicesContentDto> UpdateAsync(UpdateLogServicesContentDto dto, CancellationToken cancellationToken = default)
    {
        LogServicesContent? entity = await _context.LogServicesContents
            .FirstOrDefaultAsync(x => x.LogServicesContentId == dto.LogServicesContentId, cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException($"LogServicesContent with id {dto.LogServicesContentId} not found");

        if (dto.EventName != null)
            entity.EventName = dto.EventName;
        if (dto.LogServicesDate.HasValue)
            entity.LogServicesDate = dto.LogServicesDate.Value;
        if (dto.LogServicesLogLevel != null)
            entity.LogServicesLogLevel = dto.LogServicesLogLevel;
        if (dto.LogServicesState != null)
            entity.LogServicesState = dto.LogServicesState;
        if (dto.LogServicesContentText != null)
            entity.LogServicesContentText = dto.LogServicesContentText;

        _context.LogServicesContents.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        await _eventPublisher.PublishLogServicesContentUpdatedAsync(new LogServicesContentUpdatedEvent
        {
            LogServicesContentId = entity.LogServicesContentId,
            LogId = entity.LogId,
            EventName = entity.EventName,
            LogServicesDate = entity.LogServicesDate,
            LogServicesLogLevel = entity.LogServicesLogLevel,
            LogServicesState = entity.LogServicesState,
            LogServicesContentText = entity.LogServicesContentText,
            UpdatedAt = DateTime.UtcNow
        });

        return _mapper.Map<LogServicesContentDto>(entity);
    }

    public async Task<BulkUpdateResultDto<LogServicesContentDto>> UpdateBulkAsync(
        IEnumerable<UpdateLogServicesContentDto> dtos,
        CancellationToken cancellationToken = default)
    {
        List<UpdateLogServicesContentDto> dtoList = dtos.ToList();

        if (dtoList.Count == 0)
        {
            return new BulkUpdateResultDto<LogServicesContentDto>
            {
                TotalRequested = 0,
                TotalUpdated = 0,
                Success = true
            };
        }

        if (dtoList.Count > 1000)
        {
            return new BulkUpdateResultDto<LogServicesContentDto>
            {
                TotalRequested = dtoList.Count,
                TotalUpdated = 0,
                Success = false,
                ErrorMessage = "El límite máximo por lote es de 1000 registros."
            };
        }

        var validDtos = new List<UpdateLogServicesContentDto>();
        var errors = new List<BulkUpdateError>();

        for (int i = 0; i < dtoList.Count; i++)
        {
            UpdateLogServicesContentDto dto = dtoList[i];
            if (dto.LogServicesContentId == Guid.Empty)
            {
                errors.Add(new BulkUpdateError
                {
                    Index = i,
                    ErrorMessage = "LogServicesContentId es requerido.",
                    FailedItem = dto
                });
            }
            else
            {
                validDtos.Add(dto);
            }
        }

        if (validDtos.Count == 0)
        {
            return new BulkUpdateResultDto<LogServicesContentDto>
            {
                TotalRequested = dtoList.Count,
                TotalUpdated = 0,
                TotalFailed = errors.Count,
                Success = false,
                ErrorMessage = "Ningún registro pasó la validación.",
                Errors = errors
            };
        }

        var strategy = _context.Database.CreateExecutionStrategy();

        try
        {
            List<LogServicesContentDto> results = await strategy.ExecuteAsync(async ct =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync(ct);
                var updatedList = new List<LogServicesContent>();

                foreach (UpdateLogServicesContentDto dto in validDtos)
                {
                    LogServicesContent? entity = await _context.LogServicesContents
                        .FirstOrDefaultAsync(x => x.LogServicesContentId == dto.LogServicesContentId, ct);

                    if (entity == null)
                    {
                        errors.Add(new BulkUpdateError
                        {
                            Index = dtoList.IndexOf(dto),
                            ErrorMessage = $"LogServicesContent con id {dto.LogServicesContentId} no encontrado.",
                            FailedItem = dto
                        });
                        continue;
                    }

                    if (dto.EventName != null)
                        entity.EventName = dto.EventName;
                    if (dto.LogServicesDate.HasValue)
                        entity.LogServicesDate = dto.LogServicesDate.Value;
                    if (dto.LogServicesLogLevel != null)
                        entity.LogServicesLogLevel = dto.LogServicesLogLevel;
                    if (dto.LogServicesState != null)
                        entity.LogServicesState = dto.LogServicesState;
                    if (dto.LogServicesContentText != null)
                        entity.LogServicesContentText = dto.LogServicesContentText;

                    _context.LogServicesContents.Update(entity);
                    updatedList.Add(entity);
                }

                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
                return _mapper.Map<List<LogServicesContentDto>>(updatedList);
            }, cancellationToken);

            // Publicar eventos en background (fire-and-forget)
            _ = Task.Run(async () =>
            {
                foreach (LogServicesContentDto result in results)
                {
                    try
                    {
                        await _eventPublisher.PublishLogServicesContentUpdatedAsync(new LogServicesContentUpdatedEvent
                        {
                            LogServicesContentId = result.LogServicesContentId,
                            LogId = result.LogId,
                            EventName = result.EventName,
                            LogServicesDate = result.LogServicesDate,
                            LogServicesLogLevel = result.LogServicesLogLevel,
                            LogServicesState = result.LogServicesState,
                            LogServicesContentText = result.LogServicesContentText,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                    catch { /* Log silencioso - no bloquea respuesta */ }
                }
            }, CancellationToken.None);

            return new BulkUpdateResultDto<LogServicesContentDto>
            {
                UpdatedItems = results,
                TotalRequested = dtoList.Count,
                TotalUpdated = results.Count,
                TotalFailed = errors.Count,
                Success = true,
                Errors = errors
            };
        }
        catch (Exception ex)
        {
            return new BulkUpdateResultDto<LogServicesContentDto>
            {
                TotalRequested = dtoList.Count,
                TotalUpdated = 0,
                TotalFailed = dtoList.Count,
                Success = false,
                ErrorMessage = ex.Message,
                Errors = errors
            };
        }
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        List<LogServicesContent> entities = await _context.LogServicesContents
            .Where(x => x.LogId == id)
            .ToListAsync(cancellationToken);

        if (entities.Count == 0)
            return false;

        _context.LogServicesContents.RemoveRange(entities);
        await _context.SaveChangesAsync(cancellationToken);

        await _eventPublisher.PublishLogServicesContentDeletedAsync(new LogServicesContentDeletedEvent
        {
            LogId = id,
            DeletedAt = DateTime.UtcNow
        });

        return true;
    }
}
