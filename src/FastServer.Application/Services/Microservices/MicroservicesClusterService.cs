using AutoMapper;
using FastServer.Application.DTOs.Microservices;
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

    public MicroservicesClusterService(IDataSourceFactory dataSourceFactory, IMapper mapper)
    {
        _dataSourceFactory = dataSourceFactory;
        _mapper = mapper;
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

        return _mapper.Map<MicroservicesClusterDto>(entity);
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

        return _mapper.Map<MicroservicesClusterDto>(entity);
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
