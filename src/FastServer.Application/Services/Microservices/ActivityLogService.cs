using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Application.EventPublishers;
using FastServer.Application.Events.ActivityLogEvents;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Services.Microservices;

/// <summary>
/// Servicio para gestionar logs de actividad en PostgreSQL (BD: FastServer)
/// </summary>
public class ActivityLogService
{
    private readonly IMicroservicesDbContext _context;
    private readonly IMapper _mapper;
    private readonly IActivityLogEventPublisher _eventPublisher;

    public ActivityLogService(
        IMicroservicesDbContext context,
        IMapper mapper,
        IActivityLogEventPublisher eventPublisher)
    {
        _context = context;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<ActivityLogDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.ActivityLogs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ActivityLogId == id, cancellationToken);
        return entity == null ? null : _mapper.Map<ActivityLogDto>(entity);
    }

    public async Task<List<ActivityLogDto>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.ActivityLogs
            .AsNoTracking()
            .Where(a => a.UserId == userId)
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<ActivityLogDto>>(entities);
    }

    public async Task<List<ActivityLogDto>> GetByEntityAsync(
        string entityName,
        Guid? entityId,
        CancellationToken cancellationToken = default)
    {
        var query = _context.ActivityLogs
            .AsNoTracking()
            .Where(a => a.ActivityLogEntityName == entityName);

        if (entityId.HasValue)
        {
            query = query.Where(a => a.ActivityLogEntityId == entityId.Value);
        }

        var entities = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<ActivityLogDto>>(entities);
    }

    public async Task<ActivityLogDto> CreateAsync(
        long? eventTypeId,
        string? entityName,
        Guid? entityId,
        string? description,
        Guid? userId,
        CancellationToken cancellationToken = default)
    {
        var entity = new ActivityLog
        {
            ActivityLogId = Guid.NewGuid(),
            EventTypeId = eventTypeId,
            ActivityLogEntityName = entityName,
            ActivityLogEntityId = entityId,
            ActivityLogDescription = description,
            UserId = userId,
            CreateAt = DateTime.UtcNow,
            ModifyAt = DateTime.UtcNow
        };

        _context.ActivityLogs.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<ActivityLogDto>(entity);

        // Crear evento con los campos correctos
        var createdEvent = new ActivityLogCreatedEvent
        {
            ActivityLogId = result.ActivityLogId,
            EventTypeId = result.EventTypeId,
            ActivityLogEntityName = result.ActivityLogEntityName,
            ActivityLogEntityId = result.ActivityLogEntityId,
            ActivityLogDescription = result.ActivityLogDescription,
            UserId = result.UserId,
            CreatedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishActivityLogCreatedAsync(createdEvent);

        return result;
    }

    public async Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.ActivityLogs
            .FirstOrDefaultAsync(x => x.ActivityLogId == id, cancellationToken);
        if (entity == null) return false;

        _context.ActivityLogs.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        // Crear evento con los campos correctos
        var deletedEvent = new ActivityLogDeletedEvent
        {
            ActivityLogId = entity.ActivityLogId,
            ActivityLogDescription = entity.ActivityLogDescription,
            DeletedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishActivityLogDeletedAsync(deletedEvent);

        return true;
    }
}
