using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Domain.Entities.Microservices;
using FastServer.Domain.Enums;
using FastServer.Domain.Interfaces;

namespace FastServer.Application.Services.Microservices;

/// <summary>
/// Servicio para gestionar credenciales de conectores del core
/// </summary>
public class CoreConnectorCredentialService
{
    private readonly IDataSourceFactory _dataSourceFactory;
    private readonly IMapper _mapper;

    public CoreConnectorCredentialService(IDataSourceFactory dataSourceFactory, IMapper mapper)
    {
        _dataSourceFactory = dataSourceFactory;
        _mapper = mapper;
    }

    public async Task<CoreConnectorCredentialDto?> GetByIdAsync(
        long id,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<CoreConnectorCredential>();
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<CoreConnectorCredentialDto>(entity);
    }

    public async Task<List<CoreConnectorCredentialDto>> GetAllAsync(
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<CoreConnectorCredential>();
        var entities = await repository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<CoreConnectorCredentialDto>>(entities);
    }

    public async Task<CoreConnectorCredentialDto> CreateAsync(
        string? user,
        string? password,
        string? key,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<CoreConnectorCredential>();

        var entity = new CoreConnectorCredential
        {
            CoreConnectorCredentialUser = user,
            CoreConnectorCredentialPass = password, // TODO: Implementar encriptación
            CoreConnectorCredentialKey = key,
            CreateAt = DateTime.UtcNow,
            ModifyAt = DateTime.UtcNow
        };

        await repository.AddAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CoreConnectorCredentialDto>(entity);
    }

    public async Task<CoreConnectorCredentialDto?> UpdateAsync(
        long id,
        string? user,
        string? password,
        string? key,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<CoreConnectorCredential>();

        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        if (user != null) entity.CoreConnectorCredentialUser = user;
        if (password != null) entity.CoreConnectorCredentialPass = password; // TODO: Implementar encriptación
        if (key != null) entity.CoreConnectorCredentialKey = key;
        entity.ModifyAt = DateTime.UtcNow;

        await repository.UpdateAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CoreConnectorCredentialDto>(entity);
    }

    public async Task<bool> DeleteAsync(
        long id,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<CoreConnectorCredential>();

        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;

        await repository.DeleteAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return true;
    }
}
