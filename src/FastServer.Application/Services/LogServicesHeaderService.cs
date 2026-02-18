using AutoMapper;
using FastServer.Application.DTOs;
using FastServer.Application.EventPublishers;
using FastServer.Application.Events.LogEvents;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Services;

/// <summary>
/// Servicio de aplicación para gestionar logs de cabecera (LogServicesHeader).
/// Proporciona operaciones CRUD y consultas especializadas usando PostgreSQL (BD: FastServer_Logs).
/// </summary>
/// <remarks>
/// Este servicio implementa la lógica de negocio para los logs de servicios.
/// Características principales:
/// - Acceso directo a PostgreSQL mediante ILogsDbContext
/// - Mapeo automático entre entidades y DTOs usando AutoMapper
/// - Paginación de resultados para optimizar rendimiento
/// - Filtrado avanzado de logs por múltiples criterios
/// - Uso de AsNoTracking() para queries de solo lectura
/// - Publicación de eventos para suscripciones GraphQL
/// </remarks>
public class LogServicesHeaderService : ILogServicesHeaderService
{
    private readonly ILogsDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogEventPublisher _eventPublisher;

    /// <summary>
    /// Inicializa una nueva instancia del servicio LogServicesHeaderService.
    /// </summary>
    /// <param name="context">Contexto de base de datos PostgreSQL para logs (FastServer_Logs)</param>
    /// <param name="mapper">Mapeador para convertir entre entidades y DTOs</param>
    /// <param name="eventPublisher">Publisher de eventos para suscripciones GraphQL</param>
    public LogServicesHeaderService(
        ILogsDbContext context,
        IMapper mapper,
        ILogEventPublisher eventPublisher)
    {
        _context = context;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// Obtiene un log por su identificador único.
    /// </summary>
    /// <param name="id">Identificador del log a buscar</param>
    /// <param name="cancellationToken">Token para cancelar la operación</param>
    /// <returns>El log encontrado o null si no existe</returns>
    public async Task<LogServicesHeaderDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.LogServicesHeaders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.LogId == id, cancellationToken);

        return entity == null ? null : _mapper.Map<LogServicesHeaderDto>(entity);
    }

    /// <summary>
    /// Obtiene un log con todos sus detalles (campos completos de la cabecera).
    /// </summary>
    /// <param name="id">Identificador del log a buscar</param>
    /// <param name="cancellationToken">Token para cancelar la operación</param>
    /// <returns>El log con todos sus campos o null si no existe</returns>
    /// <remarks>
    /// Este método es equivalente a GetByIdAsync. Las relaciones con LogMicroservices y
    /// LogServicesContents deben obtenerse por separado usando sus respectivos servicios.
    /// </remarks>
    public async Task<LogServicesHeaderDto?> GetWithDetailsAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.LogServicesHeaders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.LogId == id, cancellationToken);

        return entity == null ? null : _mapper.Map<LogServicesHeaderDto>(entity);
    }

    public async Task<PaginatedResultDto<LogServicesHeaderDto>> GetAllAsync(PaginationParamsDto pagination, CancellationToken cancellationToken = default)
    {
        var totalCount = await _context.LogServicesHeaders.CountAsync(cancellationToken);

        var entities = await _context.LogServicesHeaders
            .AsNoTracking()
            .Skip(pagination.Skip)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResultDto<LogServicesHeaderDto>
        {
            Items = _mapper.Map<IEnumerable<LogServicesHeaderDto>>(entities),
            TotalCount = totalCount,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize
        };
    }

    public async Task<PaginatedResultDto<LogServicesHeaderDto>> GetByFilterAsync(LogFilterDto filter, PaginationParamsDto pagination, CancellationToken cancellationToken = default)
    {
        var query = _context.LogServicesHeaders.AsNoTracking();

        if (filter.StartDate.HasValue)
            query = query.Where(x => x.LogDateIn >= filter.StartDate.Value);

        if (filter.EndDate.HasValue)
            query = query.Where(x => x.LogDateIn <= filter.EndDate.Value);

        if (filter.State.HasValue)
            query = query.Where(x => x.LogState == filter.State.Value);

        if (!string.IsNullOrEmpty(filter.MicroserviceName))
            query = query.Where(x => x.MicroserviceName != null && x.MicroserviceName.Contains(filter.MicroserviceName));

        if (!string.IsNullOrEmpty(filter.UserId))
            query = query.Where(x => x.UserId == filter.UserId);

        if (!string.IsNullOrEmpty(filter.TransactionId))
            query = query.Where(x => x.TransactionId == filter.TransactionId);

        if (!string.IsNullOrEmpty(filter.HttpMethod))
            query = query.Where(x => x.HttpMethod == filter.HttpMethod);

        if (filter.HasErrors.HasValue && filter.HasErrors.Value)
            query = query.Where(x => x.ErrorCode != null);

        var totalCount = await query.CountAsync(cancellationToken);
        var entities = await query
            .OrderByDescending(x => x.LogDateIn)
            .Skip(pagination.Skip)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResultDto<LogServicesHeaderDto>
        {
            Items = _mapper.Map<IEnumerable<LogServicesHeaderDto>>(entities),
            TotalCount = totalCount,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize
        };
    }

    public async Task<BulkInsertResultDto<LogServicesHeaderDto>> CreateBulkAsync(
        IEnumerable<CreateLogServicesHeaderDto> dtos,
        CancellationToken cancellationToken = default)
    {
        var dtoList = dtos.ToList();

        if (dtoList.Count == 0)
        {
            return new BulkInsertResultDto<LogServicesHeaderDto>
            {
                TotalRequested = 0,
                TotalInserted = 0,
                Success = true
            };
        }

        if (dtoList.Count > 1000)
        {
            return new BulkInsertResultDto<LogServicesHeaderDto>
            {
                TotalRequested = dtoList.Count,
                TotalInserted = 0,
                Success = false,
                ErrorMessage = "El límite máximo por lote es de 1000 registros."
            };
        }

        // Validar cada registro y separar válidos de inválidos
        var validDtos = new List<CreateLogServicesHeaderDto>();
        var errors = new List<BulkInsertError>();

        for (int i = 0; i < dtoList.Count; i++)
        {
            var dto = dtoList[i];
            var validationError = ValidateLogServicesHeader(dto);
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

        // Si no hay registros válidos, retornar solo los errores
        if (validDtos.Count == 0)
        {
            return new BulkInsertResultDto<LogServicesHeaderDto>
            {
                TotalRequested = dtoList.Count,
                TotalInserted = 0,
                TotalFailed = errors.Count,
                Success = false,
                ErrorMessage = "Ningún registro pasó la validación.",
                Errors = errors
            };
        }

        var entities = _mapper.Map<List<LogServicesHeader>>(validDtos);
        var strategy = _context.Database.CreateExecutionStrategy();

        try
        {
            var results = await strategy.ExecuteAsync(async ct =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync(ct);
                await _context.LogServicesHeaders.AddRangeAsync(entities, ct);
                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
                return _mapper.Map<List<LogServicesHeaderDto>>(entities);
            }, cancellationToken);

            // Publicar eventos en background (fire-and-forget)
            _ = Task.Run(async () =>
            {
                foreach (var entity in entities)
                {
                    try
                    {
                        await _eventPublisher.PublishLogCreatedAsync(new LogCreatedEvent
                        {
                            LogId = entity.LogId,
                            LogDateIn = entity.LogDateIn,
                            LogDateOut = entity.LogDateOut,
                            LogState = entity.LogState,
                            LogMethodUrl = entity.LogMethodUrl,
                            LogMethodName = entity.LogMethodName,
                            LogFsId = entity.LogFsId,
                            MethodDescription = entity.MethodDescription,
                            TciIpPort = entity.TciIpPort,
                            ErrorCode = entity.ErrorCode,
                            ErrorDescription = entity.ErrorDescription,
                            IpFs = entity.IpFs,
                            TypeProcess = entity.TypeProcess,
                            LogNodo = entity.LogNodo,
                            HttpMethod = entity.HttpMethod,
                            MicroserviceName = entity.MicroserviceName,
                            RequestDuration = entity.RequestDuration,
                            TransactionId = entity.TransactionId,
                            UserId = entity.UserId,
                            SessionId = entity.SessionId,
                            RequestId = entity.RequestId,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                    catch { /* Log silencioso - no bloquea respuesta */ }
                }
            }, CancellationToken.None);

            return new BulkInsertResultDto<LogServicesHeaderDto>
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
            return new BulkInsertResultDto<LogServicesHeaderDto>
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

    private static string? ValidateLogServicesHeader(CreateLogServicesHeaderDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.LogMethodUrl))
            return "LogMethodUrl es requerido.";
        if (dto.LogDateIn == default)
            return "LogDateIn es requerido.";
        if (dto.LogDateOut == default)
            return "LogDateOut es requerido.";
        if (dto.LogDateOut < dto.LogDateIn)
            return "LogDateOut no puede ser anterior a LogDateIn.";
        return null;
    }

    public async Task<LogServicesHeaderDto> CreateAsync(CreateLogServicesHeaderDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<LogServicesHeader>(dto);
        await _context.LogServicesHeaders.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // Publicar evento de creación
        var logEvent = new LogCreatedEvent
        {
            LogId = entity.LogId,
            LogDateIn = entity.LogDateIn,
            LogDateOut = entity.LogDateOut,
            LogState = entity.LogState,
            LogMethodUrl = entity.LogMethodUrl,
            LogMethodName = entity.LogMethodName,
            LogFsId = entity.LogFsId,
            MethodDescription = entity.MethodDescription,
            TciIpPort = entity.TciIpPort,
            ErrorCode = entity.ErrorCode,
            ErrorDescription = entity.ErrorDescription,
            IpFs = entity.IpFs,
            TypeProcess = entity.TypeProcess,
            LogNodo = entity.LogNodo,
            HttpMethod = entity.HttpMethod,
            MicroserviceName = entity.MicroserviceName,
            RequestDuration = entity.RequestDuration,
            TransactionId = entity.TransactionId,
            UserId = entity.UserId,
            SessionId = entity.SessionId,
            RequestId = entity.RequestId,
            CreatedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishLogCreatedAsync(logEvent);

        return _mapper.Map<LogServicesHeaderDto>(entity);
    }

    public async Task<LogServicesHeaderDto> UpdateAsync(UpdateLogServicesHeaderDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _context.LogServicesHeaders
            .FirstOrDefaultAsync(x => x.LogId == dto.LogId, cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException($"LogServicesHeader with id {dto.LogId} not found");

        if (dto.LogDateOut.HasValue)
            entity.LogDateOut = dto.LogDateOut.Value;
        if (dto.LogState.HasValue)
            entity.LogState = dto.LogState.Value;
        if (dto.LogMethodName != null)
            entity.LogMethodName = dto.LogMethodName;
        if (dto.MethodDescription != null)
            entity.MethodDescription = dto.MethodDescription;
        if (dto.TciIpPort != null)
            entity.TciIpPort = dto.TciIpPort;
        if (dto.IpFs != null)
            entity.IpFs = dto.IpFs;
        if (dto.TypeProcess != null)
            entity.TypeProcess = dto.TypeProcess;
        if (dto.LogNodo != null)
            entity.LogNodo = dto.LogNodo;
        if (dto.MicroserviceName != null)
            entity.MicroserviceName = dto.MicroserviceName;
        if (dto.UserId != null)
            entity.UserId = dto.UserId;
        if (dto.ErrorCode != null)
            entity.ErrorCode = dto.ErrorCode;
        if (dto.ErrorDescription != null)
            entity.ErrorDescription = dto.ErrorDescription;
        if (dto.RequestDuration.HasValue)
            entity.RequestDuration = dto.RequestDuration.Value;

        _context.LogServicesHeaders.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        // Publicar evento de actualización
        var logEvent = new LogUpdatedEvent
        {
            LogId = entity.LogId,
            LogDateIn = entity.LogDateIn,
            LogDateOut = entity.LogDateOut,
            LogState = entity.LogState,
            LogMethodUrl = entity.LogMethodUrl,
            LogMethodName = entity.LogMethodName,
            LogFsId = entity.LogFsId,
            MethodDescription = entity.MethodDescription,
            TciIpPort = entity.TciIpPort,
            ErrorCode = entity.ErrorCode,
            ErrorDescription = entity.ErrorDescription,
            IpFs = entity.IpFs,
            TypeProcess = entity.TypeProcess,
            LogNodo = entity.LogNodo,
            HttpMethod = entity.HttpMethod,
            MicroserviceName = entity.MicroserviceName,
            RequestDuration = entity.RequestDuration,
            TransactionId = entity.TransactionId,
            UserId = entity.UserId,
            SessionId = entity.SessionId,
            RequestId = entity.RequestId,
            UpdatedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishLogUpdatedAsync(logEvent);

        return _mapper.Map<LogServicesHeaderDto>(entity);
    }

    public async Task<BulkUpdateResultDto<LogServicesHeaderDto>> UpdateBulkAsync(
        IEnumerable<UpdateLogServicesHeaderDto> dtos,
        CancellationToken cancellationToken = default)
    {
        var dtoList = dtos.ToList();

        if (dtoList.Count == 0)
        {
            return new BulkUpdateResultDto<LogServicesHeaderDto>
            {
                TotalRequested = 0,
                TotalUpdated = 0,
                Success = true
            };
        }

        if (dtoList.Count > 1000)
        {
            return new BulkUpdateResultDto<LogServicesHeaderDto>
            {
                TotalRequested = dtoList.Count,
                TotalUpdated = 0,
                Success = false,
                ErrorMessage = "El límite máximo por lote es de 1000 registros."
            };
        }

        var validDtos = new List<UpdateLogServicesHeaderDto>();
        var errors = new List<BulkUpdateError>();

        for (int i = 0; i < dtoList.Count; i++)
        {
            var dto = dtoList[i];
            if (dto.LogId <= 0)
            {
                errors.Add(new BulkUpdateError
                {
                    Index = i,
                    ErrorMessage = "LogId debe ser mayor a 0.",
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
            return new BulkUpdateResultDto<LogServicesHeaderDto>
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
                var updatedList = new List<LogServicesHeader>();

                foreach (var dto in validDtos)
                {
                    var entity = await _context.LogServicesHeaders
                        .FirstOrDefaultAsync(x => x.LogId == dto.LogId, ct);

                    if (entity == null)
                    {
                        errors.Add(new BulkUpdateError
                        {
                            Index = dtoList.IndexOf(dto),
                            ErrorMessage = $"LogServicesHeader con id {dto.LogId} no encontrado.",
                            FailedItem = dto
                        });
                        continue;
                    }

                    if (dto.LogDateOut.HasValue)
                        entity.LogDateOut = dto.LogDateOut.Value;
                    if (dto.LogState.HasValue)
                        entity.LogState = dto.LogState.Value;
                    if (dto.LogMethodName != null)
                        entity.LogMethodName = dto.LogMethodName;
                    if (dto.MethodDescription != null)
                        entity.MethodDescription = dto.MethodDescription;
                    if (dto.TciIpPort != null)
                        entity.TciIpPort = dto.TciIpPort;
                    if (dto.IpFs != null)
                        entity.IpFs = dto.IpFs;
                    if (dto.TypeProcess != null)
                        entity.TypeProcess = dto.TypeProcess;
                    if (dto.LogNodo != null)
                        entity.LogNodo = dto.LogNodo;
                    if (dto.MicroserviceName != null)
                        entity.MicroserviceName = dto.MicroserviceName;
                    if (dto.UserId != null)
                        entity.UserId = dto.UserId;
                    if (dto.ErrorCode != null)
                        entity.ErrorCode = dto.ErrorCode;
                    if (dto.ErrorDescription != null)
                        entity.ErrorDescription = dto.ErrorDescription;
                    if (dto.RequestDuration.HasValue)
                        entity.RequestDuration = dto.RequestDuration.Value;

                    _context.LogServicesHeaders.Update(entity);
                    updatedList.Add(entity);
                }

                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
                return _mapper.Map<List<LogServicesHeaderDto>>(updatedList);
            }, cancellationToken);

            // Publicar eventos en background (fire-and-forget)
            _ = Task.Run(async () =>
            {
                foreach (var entity in results)
                {
                    try
                    {
                        await _eventPublisher.PublishLogUpdatedAsync(new LogUpdatedEvent
                        {
                            LogId = entity.LogId,
                            LogDateIn = entity.LogDateIn,
                            LogDateOut = entity.LogDateOut,
                            LogState = entity.LogState,
                            LogMethodUrl = entity.LogMethodUrl,
                            LogMethodName = entity.LogMethodName,
                            LogFsId = entity.LogFsId,
                            MethodDescription = entity.MethodDescription,
                            TciIpPort = entity.TciIpPort,
                            ErrorCode = entity.ErrorCode,
                            ErrorDescription = entity.ErrorDescription,
                            IpFs = entity.IpFs,
                            TypeProcess = entity.TypeProcess,
                            LogNodo = entity.LogNodo,
                            HttpMethod = entity.HttpMethod,
                            MicroserviceName = entity.MicroserviceName,
                            RequestDuration = entity.RequestDuration,
                            TransactionId = entity.TransactionId,
                            UserId = entity.UserId,
                            SessionId = entity.SessionId,
                            RequestId = entity.RequestId,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                    catch { /* Log silencioso - no bloquea respuesta */ }
                }
            }, CancellationToken.None);

            return new BulkUpdateResultDto<LogServicesHeaderDto>
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
            return new BulkUpdateResultDto<LogServicesHeaderDto>
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
        var entity = await _context.LogServicesHeaders
            .FirstOrDefaultAsync(x => x.LogId == id, cancellationToken);

        if (entity == null)
            return false;

        _context.LogServicesHeaders.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        // Publicar evento de eliminación
        var logEvent = new LogDeletedEvent
        {
            LogId = entity.LogId,
            MicroserviceName = entity.MicroserviceName,
            UserId = entity.UserId,
            DeletedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishLogDeletedAsync(logEvent);

        return true;
    }

    public async Task<IEnumerable<LogServicesHeaderDto>> GetFailedLogsAsync(DateTime? fromDate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.LogServicesHeaders
            .AsNoTracking()
            .Where(x => x.ErrorCode != null);

        if (fromDate.HasValue)
            query = query.Where(x => x.LogDateIn >= fromDate.Value);

        var entities = await query
            .OrderByDescending(x => x.LogDateIn)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<LogServicesHeaderDto>>(entities);
    }
}
