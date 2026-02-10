using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Application.EventPublishers;
using FastServer.Application.Events.MicroserviceRegisterEvents;
using FastServer.Domain.Entities.Microservices;
using FastServer.Domain.Enums;
using FastServer.Domain.Interfaces;

namespace FastServer.Application.Services.Microservices;

/// <summary>
/// Servicio para gestionar registros de microservicios
/// </summary>
public class MicroserviceRegisterService
{
    private readonly IDataSourceFactory _dataSourceFactory;
    private readonly IMapper _mapper;
    private readonly IMicroserviceRegisterEventPublisher _eventPublisher;

    public MicroserviceRegisterService(
        IDataSourceFactory dataSourceFactory,
        IMapper mapper,
        IMicroserviceRegisterEventPublisher eventPublisher)
    {
        _dataSourceFactory = dataSourceFactory;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<MicroserviceRegisterDto?> GetByIdAsync(
        long id,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroserviceRegister>();
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<MicroserviceRegisterDto>(entity);
    }

    public async Task<List<MicroserviceRegisterDto>> GetAllAsync(
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroserviceRegister>();
        var entities = await repository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<MicroserviceRegisterDto>>(entities);
    }

    public async Task<List<MicroserviceRegisterDto>> GetAllActiveAsync(
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroserviceRegister>();
        var entities = await repository.FindAsync(
            m => m.MicroserviceActive == true && m.MicroserviceDeleted != true,
            cancellationToken);
        return _mapper.Map<List<MicroserviceRegisterDto>>(entities);
    }

    public async Task<List<MicroserviceRegisterDto>> GetByClusterIdAsync(
        long clusterId,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroserviceRegister>();
        var entities = await repository.FindAsync(
            m => m.MicroserviceClusterId == clusterId && m.MicroserviceDeleted != true,
            cancellationToken);
        return _mapper.Map<List<MicroserviceRegisterDto>>(entities);
    }

    public async Task<MicroserviceRegisterDto> CreateAsync(
        long? clusterId,
        string? name,
        bool active,
        bool coreConnection,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroserviceRegister>();

        var entity = new MicroserviceRegister
        {
            MicroserviceClusterId = clusterId,
            MicroserviceName = name,
            MicroserviceActive = active,
            MicroserviceDeleted = false,
            MicroserviceCoreConnection = coreConnection,
            CreateAt = DateTime.UtcNow,
            ModifyAt = DateTime.UtcNow
        };

        await repository.AddAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<MicroserviceRegisterDto>(entity);

        // Crear evento con los campos correctos
        var createdEvent = new MicroserviceRegisterCreatedEvent
        {
            MicroserviceId = result.MicroserviceId,
            MicroserviceClusterId = result.MicroserviceClusterId,
            MicroserviceName = result.MicroserviceName,
            MicroserviceActive = result.MicroserviceActive,
            MicroserviceDeleted = result.MicroserviceDeleted,
            MicroserviceCoreConnection = result.MicroserviceCoreConnection,
            DeleteAt = result.DeleteAt,
            CreatedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishMicroserviceRegisterCreatedAsync(createdEvent);

        return result;
    }

    public async Task<MicroserviceRegisterDto?> UpdateAsync(
        long id,
        long? clusterId,
        string? name,
        bool? active,
        bool? coreConnection,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroserviceRegister>();

        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        if (clusterId.HasValue) entity.MicroserviceClusterId = clusterId;
        if (name != null) entity.MicroserviceName = name;
        if (active.HasValue) entity.MicroserviceActive = active.Value;
        if (coreConnection.HasValue) entity.MicroserviceCoreConnection = coreConnection.Value;
        entity.ModifyAt = DateTime.UtcNow;

        await repository.UpdateAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<MicroserviceRegisterDto>(entity);

        // Crear evento con los campos correctos
        var updatedEvent = new MicroserviceRegisterUpdatedEvent
        {
            MicroserviceId = result.MicroserviceId,
            MicroserviceClusterId = result.MicroserviceClusterId,
            MicroserviceName = result.MicroserviceName,
            MicroserviceActive = result.MicroserviceActive,
            MicroserviceDeleted = result.MicroserviceDeleted,
            MicroserviceCoreConnection = result.MicroserviceCoreConnection,
            DeleteAt = result.DeleteAt,
            UpdatedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishMicroserviceRegisterUpdatedAsync(updatedEvent);

        return result;
    }

    public async Task<bool> SoftDeleteAsync(
        long id,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroserviceRegister>();

        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;

        entity.MicroserviceDeleted = true;
        entity.DeleteAt = DateTime.UtcNow;
        entity.ModifyAt = DateTime.UtcNow;

        await repository.UpdateAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        // Crear evento con los campos correctos
        var deletedEvent = new MicroserviceRegisterDeletedEvent
        {
            MicroserviceId = entity.MicroserviceId,
            MicroserviceName = entity.MicroserviceName,
            DeletedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishMicroserviceRegisterDeletedAsync(deletedEvent);

        return true;
    }

    public async Task<bool> SetActiveAsync(
        long id,
        bool active,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroserviceRegister>();

        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;

        entity.MicroserviceActive = active;
        entity.ModifyAt = DateTime.UtcNow;

        await repository.UpdateAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return true;
    }
}
