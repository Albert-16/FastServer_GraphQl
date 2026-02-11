using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Application.EventPublishers;
using FastServer.Application.Events.MicroservicesClusterEvents;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Services.Microservices;

/// <summary>
/// Servicio para gestionar clusters de microservicios en PostgreSQL (BD: FastServer)
/// </summary>
public class MicroservicesClusterService
{
    private readonly IMicroservicesDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMicroservicesClusterEventPublisher _eventPublisher;

    public MicroservicesClusterService(
        IMicroservicesDbContext context,
        IMapper mapper,
        IMicroservicesClusterEventPublisher eventPublisher)
    {
        _context = context;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<MicroservicesClusterDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.MicroservicesClusters
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MicroservicesClusterId == id, cancellationToken);
        return entity == null ? null : _mapper.Map<MicroservicesClusterDto>(entity);
    }

    public async Task<List<MicroservicesClusterDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.MicroservicesClusters
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<MicroservicesClusterDto>>(entities);
    }

    public async Task<List<MicroservicesClusterDto>> GetAllActiveAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.MicroservicesClusters
            .AsNoTracking()
            .Where(c => c.MicroservicesClusterActive == true && c.MicroservicesClusterDeleted != true)
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<MicroservicesClusterDto>>(entities);
    }

    public async Task<MicroservicesClusterDto> CreateAsync(
        string? name,
        string? serverName,
        string? serverIp,
        bool active,
        CancellationToken cancellationToken = default)
    {
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

        _context.MicroservicesClusters.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

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
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.MicroservicesClusters
            .FirstOrDefaultAsync(x => x.MicroservicesClusterId == id, cancellationToken);
        if (entity == null) return null;

        if (name != null) entity.MicroservicesClusterName = name;
        if (serverName != null) entity.MicroservicesClusterServerName = serverName;
        if (serverIp != null) entity.MicroservicesClusterServerIp = serverIp;
        if (active.HasValue) entity.MicroservicesClusterActive = active.Value;
        entity.ModifyAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

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
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.MicroservicesClusters
            .FirstOrDefaultAsync(x => x.MicroservicesClusterId == id, cancellationToken);
        if (entity == null) return false;

        entity.MicroservicesClusterDeleted = true;
        entity.DeleteAt = DateTime.UtcNow;
        entity.ModifyAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

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
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.MicroservicesClusters
            .FirstOrDefaultAsync(x => x.MicroservicesClusterId == id, cancellationToken);
        if (entity == null) return false;

        entity.MicroservicesClusterActive = active;
        entity.ModifyAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
