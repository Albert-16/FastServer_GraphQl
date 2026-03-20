using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Application.EventPublishers;
using FastServer.Application.Events.FastServerClusterEvents;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Services.Microservices;

/// <summary>
/// Servicio para gestionar clusters de FastServer en PostgreSQL (BD: FastServer).
/// Usa Guid v7 como PK y DateTimeOffset para timestamps.
/// </summary>
public class FastServerClusterService
{
    private readonly IMicroservicesDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFastServerClusterEventPublisher _eventPublisher;

    public FastServerClusterService(
        IMicroservicesDbContext context,
        IMapper mapper,
        IFastServerClusterEventPublisher eventPublisher)
    {
        _context = context;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// Obtiene un cluster de FastServer por su ID
    /// </summary>
    public async Task<FastServerClusterDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        FastServerCluster? entity = await _context.FastServerClusters
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.FastServerClusterId == id, cancellationToken);
        return entity == null ? null : _mapper.Map<FastServerClusterDto>(entity);
    }

    /// <summary>
    /// Obtiene todos los clusters de FastServer
    /// </summary>
    public async Task<List<FastServerClusterDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        List<FastServerCluster> entities = await _context.FastServerClusters
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<FastServerClusterDto>>(entities);
    }

    /// <summary>
    /// Obtiene todos los clusters activos de FastServer
    /// </summary>
    public async Task<List<FastServerClusterDto>> GetAllActiveAsync(
        CancellationToken cancellationToken = default)
    {
        List<FastServerCluster> entities = await _context.FastServerClusters
            .AsNoTracking()
            .Where(c => c.FastServerClusterActive == true && c.FastServerClusterDelete != true)
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<FastServerClusterDto>>(entities);
    }

    /// <summary>
    /// Crea un nuevo cluster de FastServer con Guid v7
    /// </summary>
    public async Task<FastServerClusterDto> CreateAsync(
        string? name,
        string? url,
        string? version,
        string? serverName,
        string? serverIp,
        bool active,
        CancellationToken cancellationToken = default)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        var entity = new FastServerCluster
        {
            FastServerClusterId = Guid.CreateVersion7(),
            FastServerClusterName = name,
            FastServerClusterUrl = url,
            FastServerClusterVersion = version,
            FastServerClusterServerName = serverName,
            FastServerClusterServerIp = serverIp,
            FastServerClusterActive = active,
            FastServerClusterDelete = false,
            CreateAt = now,
            ModifyAt = now
        };

        _context.FastServerClusters.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        FastServerClusterDto result = _mapper.Map<FastServerClusterDto>(entity);

        var createdEvent = new FastServerClusterCreatedEvent
        {
            FastServerClusterId = result.FastServerClusterId,
            FastServerClusterName = result.FastServerClusterName,
            FastServerClusterUrl = result.FastServerClusterUrl,
            FastServerClusterVersion = result.FastServerClusterVersion,
            FastServerClusterServerName = result.FastServerClusterServerName,
            FastServerClusterServerIp = result.FastServerClusterServerIp,
            FastServerClusterActive = result.FastServerClusterActive,
            FastServerClusterDelete = result.FastServerClusterDelete,
            DeleteAt = result.DeleteAt,
            CreatedAt = now
        };
        await _eventPublisher.PublishFastServerClusterCreatedAsync(createdEvent);

        return result;
    }

    /// <summary>
    /// Actualiza un cluster de FastServer existente. Solo modifica campos proporcionados.
    /// </summary>
    public async Task<FastServerClusterDto?> UpdateAsync(
        Guid id,
        string? name,
        string? url,
        string? version,
        string? serverName,
        string? serverIp,
        bool? active,
        CancellationToken cancellationToken = default)
    {
        FastServerCluster? entity = await _context.FastServerClusters
            .FirstOrDefaultAsync(x => x.FastServerClusterId == id, cancellationToken);
        if (entity == null) return null;

        if (name != null) entity.FastServerClusterName = name;
        if (url != null) entity.FastServerClusterUrl = url;
        if (version != null) entity.FastServerClusterVersion = version;
        if (serverName != null) entity.FastServerClusterServerName = serverName;
        if (serverIp != null) entity.FastServerClusterServerIp = serverIp;
        if (active.HasValue) entity.FastServerClusterActive = active.Value;
        entity.ModifyAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        FastServerClusterDto result = _mapper.Map<FastServerClusterDto>(entity);

        var updatedEvent = new FastServerClusterUpdatedEvent
        {
            FastServerClusterId = result.FastServerClusterId,
            FastServerClusterName = result.FastServerClusterName,
            FastServerClusterUrl = result.FastServerClusterUrl,
            FastServerClusterVersion = result.FastServerClusterVersion,
            FastServerClusterServerName = result.FastServerClusterServerName,
            FastServerClusterServerIp = result.FastServerClusterServerIp,
            FastServerClusterActive = result.FastServerClusterActive,
            FastServerClusterDelete = result.FastServerClusterDelete,
            DeleteAt = result.DeleteAt,
            UpdatedAt = DateTimeOffset.UtcNow
        };
        await _eventPublisher.PublishFastServerClusterUpdatedAsync(updatedEvent);

        return result;
    }

    /// <summary>
    /// Elimina lógicamente un cluster de FastServer (soft delete)
    /// </summary>
    public async Task<bool> SoftDeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        FastServerCluster? entity = await _context.FastServerClusters
            .FirstOrDefaultAsync(x => x.FastServerClusterId == id, cancellationToken);
        if (entity == null) return false;

        DateTimeOffset now = DateTimeOffset.UtcNow;
        entity.FastServerClusterDelete = true;
        entity.DeleteAt = now;
        entity.ModifyAt = now;

        await _context.SaveChangesAsync(cancellationToken);

        var deletedEvent = new FastServerClusterDeletedEvent
        {
            FastServerClusterId = entity.FastServerClusterId,
            FastServerClusterName = entity.FastServerClusterName,
            DeletedAt = now
        };
        await _eventPublisher.PublishFastServerClusterDeletedAsync(deletedEvent);

        return true;
    }

    /// <summary>
    /// Activa o desactiva un cluster de FastServer
    /// </summary>
    public async Task<bool> SetActiveAsync(
        Guid id,
        bool active,
        CancellationToken cancellationToken = default)
    {
        FastServerCluster? entity = await _context.FastServerClusters
            .FirstOrDefaultAsync(x => x.FastServerClusterId == id, cancellationToken);
        if (entity == null) return false;

        entity.FastServerClusterActive = active;
        entity.ModifyAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
