using AutoMapper;
using FastServer.Application.DTOs;
using FastServer.Application.EventPublishers;
using FastServer.Application.Events.LogEvents;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities;
using FastServer.Domain.Enums;
using FastServer.Domain.Interfaces;
using FastServer.Domain;

namespace FastServer.Application.Services;

/// <summary>
/// Servicio de aplicación para gestionar logs de cabecera (LogServicesHeader).
/// Proporciona operaciones CRUD y consultas especializadas con soporte multi-base de datos.
/// </summary>
/// <remarks>
/// Este servicio implementa la lógica de negocio para los logs de servicios.
/// Características principales:
/// - Soporte para múltiples orígenes de datos (PostgreSQL y SQL Server)
/// - Mapeo automático entre entidades y DTOs usando AutoMapper
/// - Paginación de resultados para optimizar rendimiento
/// - Filtrado avanzado de logs por múltiples criterios
/// - Uso del patrón Unit of Work para garantizar transaccionalidad
///
/// Flujo típico de una operación:
/// 1. El servicio recibe una solicitud (con o sin especificación de origen de datos)
/// 2. Crea un UnitOfWork para el origen de datos apropiado
/// 3. Ejecuta la operación a través de los repositorios
/// 4. Mapea el resultado de entidad a DTO
/// 5. Retorna el DTO al cliente
/// </remarks>
public class LogServicesHeaderService : ILogServicesHeaderService
{
    private readonly IDataSourceFactory _dataSourceFactory;
    private readonly IMapper _mapper;
    private readonly DataSourceType _defaultDataSource;
    private readonly ILogEventPublisher _eventPublisher;

    /// <summary>
    /// Inicializa una nueva instancia del servicio LogServicesHeaderService.
    /// </summary>
    /// <param name="dataSourceFactory">Fábrica para crear unidades de trabajo por origen de datos</param>
    /// <param name="mapper">Mapeador para convertir entre entidades y DTOs</param>
    /// <param name="dataSourceSettings">Configuración del origen de datos predeterminado</param>
    /// <param name="eventPublisher">Publisher de eventos para suscripciones GraphQL</param>
    /// <remarks>
    /// El origen de datos predeterminado se obtiene de dataSourceSettings, que está
    /// configurado en appsettings.json. Esto permite que las queries sin especificar
    /// dataSource usen automáticamente la base de datos configurada.
    /// </remarks>
    public LogServicesHeaderService(
        IDataSourceFactory dataSourceFactory,
        IMapper mapper,
        DataSourceSettings dataSourceSettings,
        ILogEventPublisher eventPublisher)
    {
        _dataSourceFactory = dataSourceFactory;
        _mapper = mapper;
        _defaultDataSource = dataSourceSettings.DefaultDataSource;
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// Obtiene un log por su identificador único.
    /// </summary>
    /// <param name="id">Identificador del log a buscar</param>
    /// <param name="dataSource">Origen de datos opcional. Si es null, usa el predeterminado</param>
    /// <param name="cancellationToken">Token para cancelar la operación</param>
    /// <returns>El log encontrado o null si no existe</returns>
    public async Task<LogServicesHeaderDto?> GetByIdAsync(long id, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        // Crear UnitOfWork con el origen de datos especificado o el predeterminado
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);

        // Buscar la entidad en el repositorio
        var entity = await uow.LogServicesHeaders.GetByIdAsync(id, cancellationToken);

        // Mapear a DTO y retornar (null si no se encontró)
        return entity == null ? null : _mapper.Map<LogServicesHeaderDto>(entity);
    }

    /// <summary>
    /// Obtiene un log con todos sus detalles relacionados (microservicios y contenidos).
    /// </summary>
    /// <param name="id">Identificador del log a buscar</param>
    /// <param name="dataSource">Origen de datos opcional. Si es null, usa el predeterminado</param>
    /// <param name="cancellationToken">Token para cancelar la operación</param>
    /// <returns>El log con sus relaciones o null si no existe</returns>
    /// <remarks>
    /// Este método carga las relaciones de navegación (LogMicroservices y LogServicesContents)
    /// mediante eager loading para evitar múltiples consultas a la base de datos.
    /// </remarks>
    public async Task<LogServicesHeaderDto?> GetWithDetailsAsync(long id, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entity = await uow.LogServicesHeaders.GetWithDetailsAsync(id, cancellationToken);
        return entity == null ? null : _mapper.Map<LogServicesHeaderDto>(entity);
    }

    public async Task<PaginatedResultDto<LogServicesHeaderDto>> GetAllAsync(PaginationParamsDto pagination, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var totalCount = await uow.LogServicesHeaders.CountAsync(cancellationToken);
        var entities = await uow.LogServicesHeaders.GetAllAsync(cancellationToken);

        var pagedEntities = entities
            .Skip(pagination.Skip)
            .Take(pagination.PageSize)
            .ToList();

        return new PaginatedResultDto<LogServicesHeaderDto>
        {
            Items = _mapper.Map<IEnumerable<LogServicesHeaderDto>>(pagedEntities),
            TotalCount = totalCount,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize
        };
    }

    public async Task<PaginatedResultDto<LogServicesHeaderDto>> GetByFilterAsync(LogFilterDto filter, PaginationParamsDto pagination, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(filter.DataSource ?? _defaultDataSource);

        var query = uow.LogServicesHeaders.Query();

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

        var totalCount = query.Count();
        var entities = query
            .OrderByDescending(x => x.LogDateIn)
            .Skip(pagination.Skip)
            .Take(pagination.PageSize)
            .ToList();

        return new PaginatedResultDto<LogServicesHeaderDto>
        {
            Items = _mapper.Map<IEnumerable<LogServicesHeaderDto>>(entities),
            TotalCount = totalCount,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize
        };
    }

    public async Task<LogServicesHeaderDto> CreateAsync(CreateLogServicesHeaderDto dto, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entity = _mapper.Map<LogServicesHeader>(dto);
        var created = await uow.LogServicesHeaders.AddAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        // Publicar evento de creación
        var logEvent = new LogCreatedEvent
        {
            LogId = created.LogId,
            LogDateIn = created.LogDateIn,
            LogDateOut = created.LogDateOut,
            LogState = created.LogState,
            LogMethodUrl = created.LogMethodUrl,
            LogMethodName = created.LogMethodName,
            LogFsId = created.LogFsId,
            MethodDescription = created.MethodDescription,
            TciIpPort = created.TciIpPort,
            ErrorCode = created.ErrorCode,
            ErrorDescription = created.ErrorDescription,
            IpFs = created.IpFs,
            TypeProcess = created.TypeProcess,
            LogNodo = created.LogNodo,
            HttpMethod = created.HttpMethod,
            MicroserviceName = created.MicroserviceName,
            RequestDuration = created.RequestDuration,
            TransactionId = created.TransactionId,
            UserId = created.UserId,
            SessionId = created.SessionId,
            RequestId = created.RequestId,
            CreatedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishLogCreatedAsync(logEvent);

        return _mapper.Map<LogServicesHeaderDto>(created);
    }

    public async Task<LogServicesHeaderDto> UpdateAsync(UpdateLogServicesHeaderDto dto, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entity = await uow.LogServicesHeaders.GetByIdAsync(dto.LogId, cancellationToken);

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

        await uow.LogServicesHeaders.UpdateAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

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

    public async Task<bool> DeleteAsync(long id, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entity = await uow.LogServicesHeaders.GetByIdAsync(id, cancellationToken);

        if (entity == null)
            return false;

        await uow.LogServicesHeaders.DeleteAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

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

    public async Task<IEnumerable<LogServicesHeaderDto>> GetFailedLogsAsync(DateTime? fromDate = null, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entities = await uow.LogServicesHeaders.GetFailedLogsAsync(fromDate, cancellationToken);
        return _mapper.Map<IEnumerable<LogServicesHeaderDto>>(entities);
    }
}
