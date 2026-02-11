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
