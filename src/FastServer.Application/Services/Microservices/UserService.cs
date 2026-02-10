using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Application.EventPublishers;
using FastServer.Application.Events.UserEvents;
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
    private readonly IUserEventPublisher _eventPublisher;

    public UserService(
        IDataSourceFactory dataSourceFactory,
        IMapper mapper,
        IUserEventPublisher eventPublisher)
    {
        _dataSourceFactory = dataSourceFactory;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
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

        var result = _mapper.Map<UserDto>(entity);

        // Crear evento con los campos correctos
        var createdEvent = new UserCreatedEvent
        {
            UserId = result.UserId,
            UserPeoplesoft = result.UserPeoplesoft,
            UserActive = result.UserActive,
            UserName = result.UserName,
            UserEmail = result.UserEmail,
            LastLogin = null,
            PasswordChangedAt = null,
            EmailConfirmed = false,
            CreatedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishUserCreatedAsync(createdEvent);

        return result;
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

        var result = _mapper.Map<UserDto>(entity);

        // Crear evento con los campos correctos
        var updatedEvent = new UserUpdatedEvent
        {
            UserId = result.UserId,
            UserPeoplesoft = result.UserPeoplesoft,
            UserActive = result.UserActive,
            UserName = result.UserName,
            UserEmail = result.UserEmail,
            LastLogin = null,
            PasswordChangedAt = null,
            EmailConfirmed = false,
            UpdatedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishUserUpdatedAsync(updatedEvent);

        return result;
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

        // Crear evento con los campos correctos
        var deletedEvent = new UserDeletedEvent
        {
            UserId = entity.UserId,
            UserEmail = entity.UserEmail,
            DeletedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishUserDeletedAsync(deletedEvent);

        return true;
    }
}
