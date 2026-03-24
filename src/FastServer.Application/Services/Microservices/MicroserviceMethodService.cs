using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Application.EventPublishers;
using FastServer.Application.Events.MicroserviceMethodEvents;
using FastServer.Application.Interfaces;
using FastServer.Application.Interfaces.Microservices;
using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Services.Microservices;

/// <summary>
/// Servicio para gestionar métodos de microservicios en PostgreSQL (BD: FastServer)
/// </summary>
public class MicroserviceMethodService : IMicroserviceMethodService
{
    private readonly IMicroservicesDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMicroserviceMethodEventPublisher _eventPublisher;

    public MicroserviceMethodService(
        IMicroservicesDbContext context,
        IMapper mapper,
        IMicroserviceMethodEventPublisher eventPublisher)
    {
        _context = context;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<MicroserviceMethodDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        MicroserviceMethod? entity = await _context.MicroserviceMethods
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MicroserviceMethodId == id, cancellationToken);
        return entity == null ? null : _mapper.Map<MicroserviceMethodDto>(entity);
    }

    public async Task<List<MicroserviceMethodDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        List<MicroserviceMethod> entities = await _context.MicroserviceMethods
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<MicroserviceMethodDto>>(entities);
    }

    public async Task<List<MicroserviceMethodDto>> GetByMicroserviceIdAsync(
        Guid microserviceId,
        CancellationToken cancellationToken = default)
    {
        List<MicroserviceMethod> entities = await _context.MicroserviceMethods
            .AsNoTracking()
            .Where(m => m.MicroserviceId == microserviceId && m.MicroserviceMethodDelete != true)
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<MicroserviceMethodDto>>(entities);
    }

    public async Task<MicroserviceMethodDto> CreateAsync(
        Guid microserviceId,
        string? name,
        string? url,
        string? httpMethod,
        CancellationToken cancellationToken = default)
    {
        var entity = new MicroserviceMethod
        {
            MicroserviceMethodId = Guid.CreateVersion7(),
            MicroserviceId = microserviceId,
            MicroserviceMethodName = name,
            MicroserviceMethodUrl = url,
            HttpMethod = httpMethod,
            MicroserviceMethodDelete = false,
            CreateAt = DateTime.UtcNow,
            ModifyAt = DateTime.UtcNow
        };

        _context.MicroserviceMethods.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        MicroserviceMethodDto result = _mapper.Map<MicroserviceMethodDto>(entity);

        var createdEvent = new MicroserviceMethodCreatedEvent
        {
            MicroserviceMethodId = result.MicroserviceMethodId,
            MicroserviceId = result.MicroserviceId,
            MicroserviceMethodName = result.MicroserviceMethodName,
            MicroserviceMethodUrl = result.MicroserviceMethodUrl,
            HttpMethod = result.HttpMethod,
            MicroserviceMethodDelete = result.MicroserviceMethodDelete,
            CreatedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishMicroserviceMethodCreatedAsync(createdEvent);

        return result;
    }

    public async Task<MicroserviceMethodDto?> UpdateAsync(
        Guid id,
        Guid? microserviceId,
        string? name,
        string? url,
        string? httpMethod,
        CancellationToken cancellationToken = default)
    {
        MicroserviceMethod? entity = await _context.MicroserviceMethods
            .FirstOrDefaultAsync(x => x.MicroserviceMethodId == id, cancellationToken);
        if (entity == null) return null;

        if (microserviceId.HasValue) entity.MicroserviceId = microserviceId.Value;
        if (name != null) entity.MicroserviceMethodName = name;
        if (url != null) entity.MicroserviceMethodUrl = url;
        if (httpMethod != null) entity.HttpMethod = httpMethod;
        entity.ModifyAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        MicroserviceMethodDto result = _mapper.Map<MicroserviceMethodDto>(entity);

        var updatedEvent = new MicroserviceMethodUpdatedEvent
        {
            MicroserviceMethodId = result.MicroserviceMethodId,
            MicroserviceId = result.MicroserviceId,
            MicroserviceMethodName = result.MicroserviceMethodName,
            MicroserviceMethodUrl = result.MicroserviceMethodUrl,
            HttpMethod = result.HttpMethod,
            MicroserviceMethodDelete = result.MicroserviceMethodDelete,
            UpdatedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishMicroserviceMethodUpdatedAsync(updatedEvent);

        return result;
    }

    public async Task<bool> SoftDeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        MicroserviceMethod? entity = await _context.MicroserviceMethods
            .FirstOrDefaultAsync(x => x.MicroserviceMethodId == id, cancellationToken);
        if (entity == null) return false;

        entity.MicroserviceMethodDelete = true;
        entity.ModifyAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        var deletedEvent = new MicroserviceMethodDeletedEvent
        {
            MicroserviceMethodId = entity.MicroserviceMethodId,
            MicroserviceMethodName = entity.MicroserviceMethodName,
            DeletedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishMicroserviceMethodDeletedAsync(deletedEvent);

        return true;
    }
}
