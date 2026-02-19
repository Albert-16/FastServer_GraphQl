using AutoMapper;
using FastServer.Application.DTOs;
using FastServer.Application.EventPublishers;
using FastServer.Application.Events.LogMicroserviceEvents;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Services;

/// <summary>
/// Servicio de aplicación para gestionar logs de microservicios (LogMicroservice).
/// Proporciona operaciones CRUD y consultas especializadas usando PostgreSQL (BD: FastServer_Logs).
/// </summary>
public class LogMicroserviceService : ILogMicroserviceService
{
    private readonly ILogsDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogMicroserviceEventPublisher _eventPublisher;

    /// <summary>
    /// Inicializa una nueva instancia del servicio LogMicroserviceService.
    /// </summary>
    /// <param name="context">Contexto de base de datos PostgreSQL para logs (FastServer_Logs)</param>
    /// <param name="mapper">Mapeador para convertir entre entidades y DTOs</param>
    /// <param name="eventPublisher">Publisher de eventos para suscripciones GraphQL</param>
    public LogMicroserviceService(
        ILogsDbContext context,
        IMapper mapper,
        ILogMicroserviceEventPublisher eventPublisher)
    {
        _context = context;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<LogMicroserviceDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.LogMicroservices
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.LogId == id, cancellationToken);

        return entity == null ? null : _mapper.Map<LogMicroserviceDto>(entity);
    }

    public async Task<IEnumerable<LogMicroserviceDto>> GetByLogIdAsync(long logId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.LogMicroservices
            .AsNoTracking()
            .Where(x => x.LogId == logId)
            .OrderBy(x => x.LogDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<LogMicroserviceDto>>(entities);
    }

    public async Task<IEnumerable<LogMicroserviceDto>> SearchByTextAsync(string searchText, CancellationToken cancellationToken = default)
    {
        var entities = await _context.LogMicroservices
            .AsNoTracking()
            .Where(x => x.LogMicroserviceText != null && x.LogMicroserviceText.Contains(searchText))
            .OrderByDescending(x => x.LogDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<LogMicroserviceDto>>(entities);
    }

    public async Task<BulkInsertResultDto<LogMicroserviceDto>> CreateBulkAsync(
        IEnumerable<CreateLogMicroserviceDto> dtos,
        CancellationToken cancellationToken = default)
    {
        var dtoList = dtos.ToList();

        if (dtoList.Count == 0)
        {
            return new BulkInsertResultDto<LogMicroserviceDto>
            {
                TotalRequested = 0,
                TotalInserted = 0,
                Success = true
            };
        }

        if (dtoList.Count > 1000)
        {
            return new BulkInsertResultDto<LogMicroserviceDto>
            {
                TotalRequested = dtoList.Count,
                TotalInserted = 0,
                Success = false,
                ErrorMessage = "El límite máximo por lote es de 1000 registros."
            };
        }

        // Validar cada registro y separar válidos de inválidos
        var validDtos = new List<CreateLogMicroserviceDto>();
        var errors = new List<BulkInsertError>();

        for (int i = 0; i < dtoList.Count; i++)
        {
            var dto = dtoList[i];
            var validationError = ValidateLogMicroservice(dto);
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
            return new BulkInsertResultDto<LogMicroserviceDto>
            {
                TotalRequested = dtoList.Count,
                TotalInserted = 0,
                TotalFailed = errors.Count,
                Success = false,
                ErrorMessage = "Ningún registro pasó la validación.",
                Errors = errors
            };
        }

        var entities = _mapper.Map<List<LogMicroservice>>(validDtos);

        // Generar LogMicroserviceId para cada entidad
        foreach (var entity in entities)
        {
            entity.LogMicroserviceId = Guid.CreateVersion7();
        }

        var strategy = _context.Database.CreateExecutionStrategy();

        try
        {
            var results = await strategy.ExecuteAsync(async ct =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync(ct);
                await _context.LogMicroservices.AddRangeAsync(entities, ct);
                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
                return _mapper.Map<List<LogMicroserviceDto>>(entities);
            }, cancellationToken);

            // Publicar eventos en background (fire-and-forget)
            _ = Task.Run(async () =>
            {
                foreach (var result in results)
                {
                    try
                    {
                        await _eventPublisher.PublishLogMicroserviceCreatedAsync(new LogMicroserviceCreatedEvent
                        {
                            LogId = result.LogId,
                            LogMicroserviceId = result.LogMicroserviceId,
                            EventName = result.EventName,
                            LogDate = result.LogDate,
                            LogLevel = result.LogLevel,
                            LogMicroserviceText = result.LogMicroserviceText,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                    catch { /* Log silencioso - no bloquea respuesta */ }
                }
            }, CancellationToken.None);

            return new BulkInsertResultDto<LogMicroserviceDto>
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
            return new BulkInsertResultDto<LogMicroserviceDto>
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

    private static string? ValidateLogMicroservice(CreateLogMicroserviceDto dto)
    {
        if (dto.LogId <= 0)
            return "LogId debe ser mayor a 0.";
        if (string.IsNullOrWhiteSpace(dto.EventName))
            return "EventName es requerido.";
        return null;
    }

    public async Task<LogMicroserviceDto> CreateAsync(CreateLogMicroserviceDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<LogMicroservice>(dto);
        entity.LogMicroserviceId = Guid.CreateVersion7();

        await _context.LogMicroservices.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<LogMicroserviceDto>(entity);

        await _eventPublisher.PublishLogMicroserviceCreatedAsync(new LogMicroserviceCreatedEvent
        {
            LogId = result.LogId,
            LogMicroserviceId = result.LogMicroserviceId,
            EventName = result.EventName,
            LogDate = result.LogDate,
            LogLevel = result.LogLevel,
            LogMicroserviceText = result.LogMicroserviceText,
            CreatedAt = DateTime.UtcNow
        });

        return result;
    }

    public async Task<LogMicroserviceDto> UpdateAsync(UpdateLogMicroserviceDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _context.LogMicroservices
            .FirstOrDefaultAsync(x => x.LogMicroserviceId == dto.LogMicroserviceId, cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException($"LogMicroservice with id {dto.LogMicroserviceId} not found");

        if (dto.EventName != null)
            entity.EventName = dto.EventName;
        if (dto.LogDate.HasValue)
            entity.LogDate = dto.LogDate.Value;
        if (dto.LogLevel != null)
            entity.LogLevel = dto.LogLevel;
        if (dto.LogMicroserviceText != null)
            entity.LogMicroserviceText = dto.LogMicroserviceText;

        _context.LogMicroservices.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        await _eventPublisher.PublishLogMicroserviceUpdatedAsync(new LogMicroserviceUpdatedEvent
        {
            LogId = entity.LogId,
            LogMicroserviceId = entity.LogMicroserviceId,
            EventName = entity.EventName,
            LogDate = entity.LogDate,
            LogLevel = entity.LogLevel,
            LogMicroserviceText = entity.LogMicroserviceText,
            CreatedAt = DateTime.UtcNow
        });

        return _mapper.Map<LogMicroserviceDto>(entity);
    }

    public async Task<BulkUpdateResultDto<LogMicroserviceDto>> UpdateBulkAsync(
        IEnumerable<UpdateLogMicroserviceDto> dtos,
        CancellationToken cancellationToken = default)
    {
        var dtoList = dtos.ToList();

        if (dtoList.Count == 0)
        {
            return new BulkUpdateResultDto<LogMicroserviceDto>
            {
                TotalRequested = 0,
                TotalUpdated = 0,
                Success = true
            };
        }

        if (dtoList.Count > 1000)
        {
            return new BulkUpdateResultDto<LogMicroserviceDto>
            {
                TotalRequested = dtoList.Count,
                TotalUpdated = 0,
                Success = false,
                ErrorMessage = "El límite máximo por lote es de 1000 registros."
            };
        }

        var validDtos = new List<UpdateLogMicroserviceDto>();
        var errors = new List<BulkUpdateError>();

        for (int i = 0; i < dtoList.Count; i++)
        {
            var dto = dtoList[i];
            if (dto.LogMicroserviceId == Guid.Empty)
            {
                errors.Add(new BulkUpdateError
                {
                    Index = i,
                    ErrorMessage = "LogMicroserviceId es requerido.",
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
            return new BulkUpdateResultDto<LogMicroserviceDto>
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
            var results = await strategy.ExecuteAsync(async ct =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync(ct);
                var updatedList = new List<LogMicroservice>();

                foreach (var dto in validDtos)
                {
                    var entity = await _context.LogMicroservices
                        .FirstOrDefaultAsync(x => x.LogMicroserviceId == dto.LogMicroserviceId, ct);

                    if (entity == null)
                    {
                        errors.Add(new BulkUpdateError
                        {
                            Index = dtoList.IndexOf(dto),
                            ErrorMessage = $"LogMicroservice con id {dto.LogMicroserviceId} no encontrado.",
                            FailedItem = dto
                        });
                        continue;
                    }

                    if (dto.EventName != null)
                        entity.EventName = dto.EventName;
                    if (dto.LogDate.HasValue)
                        entity.LogDate = dto.LogDate.Value;
                    if (dto.LogLevel != null)
                        entity.LogLevel = dto.LogLevel;
                    if (dto.LogMicroserviceText != null)
                        entity.LogMicroserviceText = dto.LogMicroserviceText;

                    _context.LogMicroservices.Update(entity);
                    updatedList.Add(entity);
                }

                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
                return _mapper.Map<List<LogMicroserviceDto>>(updatedList);
            }, cancellationToken);

            // Publicar eventos en background (fire-and-forget)
            _ = Task.Run(async () =>
            {
                foreach (var result in results)
                {
                    try
                    {
                        await _eventPublisher.PublishLogMicroserviceUpdatedAsync(new LogMicroserviceUpdatedEvent
                        {
                            LogId = result.LogId,
                            LogMicroserviceId = result.LogMicroserviceId,
                            EventName = result.EventName,
                            LogDate = result.LogDate,
                            LogLevel = result.LogLevel,
                            LogMicroserviceText = result.LogMicroserviceText,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                    catch { /* Log silencioso - no bloquea respuesta */ }
                }
            }, CancellationToken.None);

            return new BulkUpdateResultDto<LogMicroserviceDto>
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
            return new BulkUpdateResultDto<LogMicroserviceDto>
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
        var entities = await _context.LogMicroservices
            .Where(x => x.LogId == id)
            .ToListAsync(cancellationToken);

        if (entities.Count == 0)
            return false;

        _context.LogMicroservices.RemoveRange(entities);
        await _context.SaveChangesAsync(cancellationToken);

        await _eventPublisher.PublishLogMicroserviceDeletedAsync(new LogMicroserviceDeletedEvent
        {
            LogId = id,
            DeletedAt = DateTime.UtcNow
        });

        return true;
    }
}
