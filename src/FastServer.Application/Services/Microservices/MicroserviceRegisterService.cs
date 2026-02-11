using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Application.EventPublishers;
using FastServer.Application.Events.MicroserviceRegisterEvents;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Services.Microservices;

/// <summary>
/// Servicio para gestionar registros de microservicios en PostgreSQL (BD: FastServer)
/// </summary>
public class MicroserviceRegisterService
{
    private readonly IMicroservicesDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMicroserviceRegisterEventPublisher _eventPublisher;

    public MicroserviceRegisterService(
        IMicroservicesDbContext context,
        IMapper mapper,
        IMicroserviceRegisterEventPublisher eventPublisher)
    {
        _context = context;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<MicroserviceRegisterDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.MicroserviceRegisters
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MicroserviceId == id, cancellationToken);
        return entity == null ? null : _mapper.Map<MicroserviceRegisterDto>(entity);
    }

    public async Task<List<MicroserviceRegisterDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.MicroserviceRegisters
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<MicroserviceRegisterDto>>(entities);
    }

    public async Task<List<MicroserviceRegisterDto>> GetAllActiveAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.MicroserviceRegisters
            .AsNoTracking()
            .Where(m => m.MicroserviceActive == true && m.MicroserviceDeleted != true)
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<MicroserviceRegisterDto>>(entities);
    }

    public async Task<List<MicroserviceRegisterDto>> GetByClusterIdAsync(
        long clusterId,
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.MicroserviceRegisters
            .AsNoTracking()
            .Where(m => m.MicroserviceClusterId == clusterId && m.MicroserviceDeleted != true)
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<MicroserviceRegisterDto>>(entities);
    }

    public async Task<MicroserviceRegisterDto> CreateAsync(
        long? clusterId,
        string? name,
        bool active,
        bool coreConnection,
        CancellationToken cancellationToken = default)
    {
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

        _context.MicroserviceRegisters.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

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
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.MicroserviceRegisters
            .FirstOrDefaultAsync(x => x.MicroserviceId == id, cancellationToken);
        if (entity == null) return null;

        if (clusterId.HasValue) entity.MicroserviceClusterId = clusterId;
        if (name != null) entity.MicroserviceName = name;
        if (active.HasValue) entity.MicroserviceActive = active.Value;
        if (coreConnection.HasValue) entity.MicroserviceCoreConnection = coreConnection.Value;
        entity.ModifyAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

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
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.MicroserviceRegisters
            .FirstOrDefaultAsync(x => x.MicroserviceId == id, cancellationToken);
        if (entity == null) return false;

        entity.MicroserviceDeleted = true;
        entity.DeleteAt = DateTime.UtcNow;
        entity.ModifyAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

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
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.MicroserviceRegisters
            .FirstOrDefaultAsync(x => x.MicroserviceId == id, cancellationToken);
        if (entity == null) return false;

        entity.MicroserviceActive = active;
        entity.ModifyAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
