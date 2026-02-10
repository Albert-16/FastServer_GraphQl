using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Application.EventPublishers;
using FastServer.Application.Events.MicroservicesClusterEvents;
using FastServer.Domain.Entities.Microservices;
using FastServer.Domain.Enums;
using FastServer.Domain.Interfaces;

namespace FastServer.Application.Services.Microservices;

/// <summary>
/// Servicio para gestionar clusters de microservicios
/// </summary>
public class MicroservicesClusterService
{
    private readonly IDataSourceFactory _dataSourceFactory;
    private readonly IMapper _mapper;
    private readonly IMicroservicesClusterEventPublisher _eventPublisher;

    public MicroservicesClusterService(
        IDataSourceFactory dataSourceFactory,
        IMapper mapper,
        IMicroservicesClusterEventPublisher eventPublisher)
    {
        _dataSourceFactory = dataSourceFactory;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<MicroservicesClusterDto?> GetByIdAsync(
        long id,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroservicesCluster>();
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<MicroservicesClusterDto>(entity);
    }

    public async Task<List<MicroservicesClusterDto>> GetAllAsync(
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroservicesCluster>();
        var entities = await repository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<MicroservicesClusterDto>>(entities);
    }

    public async Task<List<MicroservicesClusterDto>> GetAllActiveAsync(
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroservicesCluster>();
        var entities = await repository.FindAsync(
            c => c.MicroservicesClusterActive == true && c.MicroservicesClusterDeleted != true,
            cancellationToken);
        return _mapper.Map<List<MicroservicesClusterDto>>(entities);
    }

    public async Task<MicroservicesClusterDto> CreateAsync(
        string? name,
        string? serverName,
        string? serverIp,
        bool active,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroservicesCluster>();

        var entity = new MicroservicesCluster
        {
            MicroservicesClusterName = name,
            MicroservicesClusterServerName = serverName,
            MicroservicesClusterServerIp = serverIp,
            MicroservicesClusterActive = active,
            MicroservicesClusterDeleted = false,
            CreateAt = DateTime.UtcNow,
            ModifyAt = DateTime.UtcNow
        };

        await repository.AddAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<MicroservicesClusterDto>(entity);

        // Crear evento con los campos correctos
        var createdEvent = new MicroservicesClusterCreatedEvent
        {
            MicroservicesClusterId = result.MicroservicesClusterId,
            MicroservicesClusterName = result.MicroservicesClusterName,
            MicroservicesClusterServerName = result.MicroservicesClusterServerName,
            MicroservicesClusterServerIp = result.MicroservicesClusterServerIp,
            MicroservicesClusterActive = result.MicroservicesClusterActive,
            MicroservicesClusterDeleted = result.MicroservicesClusterDeleted,
            DeleteAt = result.DeleteAt,
            CreatedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishMicroservicesClusterCreatedAsync(createdEvent);

        return result;
    }

    public async Task<MicroservicesClusterDto?> UpdateAsync(
        long id,
        string? name,
        string? serverName,
        string? serverIp,
        bool? active,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroservicesCluster>();

        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        if (name != null) entity.MicroservicesClusterName = name;
        if (serverName != null) entity.MicroservicesClusterServerName = serverName;
        if (serverIp != null) entity.MicroservicesClusterServerIp = serverIp;
        if (active.HasValue) entity.MicroservicesClusterActive = active.Value;
        entity.ModifyAt = DateTime.UtcNow;

        await repository.UpdateAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<MicroservicesClusterDto>(entity);

        // Crear evento con los campos correctos
        var updatedEvent = new MicroservicesClusterUpdatedEvent
        {
            MicroservicesClusterId = result.MicroservicesClusterId,
            MicroservicesClusterName = result.MicroservicesClusterName,
            MicroservicesClusterServerName = result.MicroservicesClusterServerName,
            MicroservicesClusterServerIp = result.MicroservicesClusterServerIp,
            MicroservicesClusterActive = result.MicroservicesClusterActive,
            MicroservicesClusterDeleted = result.MicroservicesClusterDeleted,
            DeleteAt = result.DeleteAt,
            UpdatedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishMicroservicesClusterUpdatedAsync(updatedEvent);

        return result;
    }

    public async Task<bool> SoftDeleteAsync(
        long id,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroservicesCluster>();

        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;

        entity.MicroservicesClusterDeleted = true;
        entity.DeleteAt = DateTime.UtcNow;
        entity.ModifyAt = DateTime.UtcNow;

        await repository.UpdateAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        // Crear evento con los campos correctos
        var deletedEvent = new MicroservicesClusterDeletedEvent
        {
            MicroservicesClusterId = entity.MicroservicesClusterId,
            MicroservicesClusterName = entity.MicroservicesClusterName,
            DeletedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishMicroservicesClusterDeletedAsync(deletedEvent);

        return true;
    }

    public async Task<bool> SetActiveAsync(
        long id,
        bool active,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroservicesCluster>();

        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;

        entity.MicroservicesClusterActive = active;
        entity.ModifyAt = DateTime.UtcNow;

        await repository.UpdateAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return true;
    }
}
