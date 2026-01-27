using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Domain.Entities.Microservices;
using FastServer.Domain.Enums;
using FastServer.Domain.Interfaces;

namespace FastServer.Application.Services.Microservices;

/// <summary>
/// Servicio para gestionar usuarios
/// </summary>
public class UserService
{
    private readonly IDataSourceFactory _dataSourceFactory;
    private readonly IMapper _mapper;

    public UserService(IDataSourceFactory dataSourceFactory, IMapper mapper)
    {
        _dataSourceFactory = dataSourceFactory;
        _mapper = mapper;
    }

    public async Task<UserDto?> GetByIdAsync(
        Guid id,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<User>();
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<UserDto>(entity);
    }

    public async Task<List<UserDto>> GetAllAsync(
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<User>();
        var entities = await repository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<UserDto>>(entities);
    }

    public async Task<List<UserDto>> GetAllActiveAsync(
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<User>();
        var entities = await repository.FindAsync(u => u.UserActive == true, cancellationToken);
        return _mapper.Map<List<UserDto>>(entities);
    }

    public async Task<UserDto> CreateAsync(
        string? peoplesoft,
        string? name,
        string? email,
        bool active,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<User>();

        var entity = new User
        {
            UserId = Guid.NewGuid(),
            UserPeoplesoft = peoplesoft,
            UserName = name,
            UserEmail = email,
            UserActive = active,
            CreateAt = DateTime.UtcNow,
            ModifyAt = DateTime.UtcNow
        };

        await repository.AddAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserDto>(entity);
    }

    public async Task<UserDto?> UpdateAsync(
        Guid id,
        string? peoplesoft,
        string? name,
        string? email,
        bool? active,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<User>();

        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        if (peoplesoft != null) entity.UserPeoplesoft = peoplesoft;
        if (name != null) entity.UserName = name;
        if (email != null) entity.UserEmail = email;
        if (active.HasValue) entity.UserActive = active.Value;
        entity.ModifyAt = DateTime.UtcNow;

        await repository.UpdateAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserDto>(entity);
    }

    public async Task<bool> SetActiveAsync(
        Guid id,
        bool active,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<User>();

        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;

        entity.UserActive = active;
        entity.ModifyAt = DateTime.UtcNow;

        await repository.UpdateAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(
        Guid id,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<User>();

        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;

        await repository.DeleteAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return true;
    }
}
