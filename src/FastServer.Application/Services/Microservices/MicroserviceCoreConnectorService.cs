using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Domain.Entities.Microservices;
using FastServer.Domain.Enums;
using FastServer.Domain.Interfaces;

namespace FastServer.Application.Services.Microservices;

/// <summary>
/// Servicio para gestionar conectores entre microservicios y el core
/// </summary>
public class MicroserviceCoreConnectorService
{
    private readonly IDataSourceFactory _dataSourceFactory;
    private readonly IMapper _mapper;

    public MicroserviceCoreConnectorService(IDataSourceFactory dataSourceFactory, IMapper mapper)
    {
        _dataSourceFactory = dataSourceFactory;
        _mapper = mapper;
    }

    public async Task<MicroserviceCoreConnectorDto?> GetByIdAsync(
        long id,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroserviceCoreConnector>();
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<MicroserviceCoreConnectorDto>(entity);
    }

    public async Task<List<MicroserviceCoreConnectorDto>> GetByMicroserviceIdAsync(
        long microserviceId,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroserviceCoreConnector>();
        var entities = await repository.FindAsync(c => c.MicroserviceId == microserviceId, cancellationToken);
        return _mapper.Map<List<MicroserviceCoreConnectorDto>>(entities);
    }

    public async Task<MicroserviceCoreConnectorDto> CreateAsync(
        long? credentialId,
        long? microserviceId,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroserviceCoreConnector>();

        var entity = new MicroserviceCoreConnector
        {
            CoreConnectorCredentialId = credentialId,
            MicroserviceId = microserviceId,
            CreateAt = DateTime.UtcNow,
            ModifyAt = DateTime.UtcNow
        };

        await repository.AddAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return _mapper.Map<MicroserviceCoreConnectorDto>(entity);
    }

    public async Task<MicroserviceCoreConnectorDto?> UpdateAsync(
        long id,
        long? credentialId,
        long? microserviceId,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroserviceCoreConnector>();

        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        if (credentialId.HasValue) entity.CoreConnectorCredentialId = credentialId;
        if (microserviceId.HasValue) entity.MicroserviceId = microserviceId;
        entity.ModifyAt = DateTime.UtcNow;

        await repository.UpdateAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return _mapper.Map<MicroserviceCoreConnectorDto>(entity);
    }

    public async Task<bool> DeleteAsync(
        long id,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<MicroserviceCoreConnector>();

        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;

        await repository.DeleteAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return true;
    }
}
