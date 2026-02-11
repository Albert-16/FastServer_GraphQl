using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Application.EventPublishers;
using FastServer.Application.Events.UserEvents;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Services.Microservices;

/// <summary>
/// Servicio para gestionar usuarios en PostgreSQL (BD: FastServer)
/// </summary>
public class UserService
{
    private readonly IMicroservicesDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUserEventPublisher _eventPublisher;

    public UserService(
        IMicroservicesDbContext context,
        IMapper mapper,
        IUserEventPublisher eventPublisher)
    {
        _context = context;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<UserDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == id, cancellationToken);
        return entity == null ? null : _mapper.Map<UserDto>(entity);
    }

    public async Task<List<UserDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<UserDto>>(entities);
    }

    public async Task<List<UserDto>> GetAllActiveAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.Users
            .AsNoTracking()
            .Where(u => u.UserActive == true)
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<UserDto>>(entities);
    }

    public async Task<UserDto> CreateAsync(
        string? peoplesoft,
        string? name,
        string? email,
        bool active,
        CancellationToken cancellationToken = default)
    {
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

        _context.Users.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

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
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Users
            .FirstOrDefaultAsync(x => x.UserId == id, cancellationToken);
        if (entity == null) return null;

        if (peoplesoft != null) entity.UserPeoplesoft = peoplesoft;
        if (name != null) entity.UserName = name;
        if (email != null) entity.UserEmail = email;
        if (active.HasValue) entity.UserActive = active.Value;
        entity.ModifyAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

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
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Users
            .FirstOrDefaultAsync(x => x.UserId == id, cancellationToken);
        if (entity == null) return false;

        entity.UserActive = active;
        entity.ModifyAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Users
            .FirstOrDefaultAsync(x => x.UserId == id, cancellationToken);
        if (entity == null) return false;

        _context.Users.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

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
