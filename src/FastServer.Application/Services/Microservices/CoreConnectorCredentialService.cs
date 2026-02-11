using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Application.EventPublishers;
using FastServer.Application.Events.CoreConnectorCredentialEvents;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Services.Microservices;

/// <summary>
/// Servicio para gestionar credenciales de conectores del core en PostgreSQL (BD: FastServer)
/// </summary>
public class CoreConnectorCredentialService
{
    private readonly IMicroservicesDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICoreConnectorCredentialEventPublisher _eventPublisher;

    public CoreConnectorCredentialService(
        IMicroservicesDbContext context,
        IMapper mapper,
        ICoreConnectorCredentialEventPublisher eventPublisher)
    {
        _context = context;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<CoreConnectorCredentialDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.CoreConnectorCredentials
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CoreConnectorCredentialId == id, cancellationToken);
        return entity == null ? null : _mapper.Map<CoreConnectorCredentialDto>(entity);
    }

    public async Task<List<CoreConnectorCredentialDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.CoreConnectorCredentials
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<CoreConnectorCredentialDto>>(entities);
    }

    public async Task<CoreConnectorCredentialDto> CreateAsync(
        string? user,
        string? password,
        string? key,
        CancellationToken cancellationToken = default)
    {
        var entity = new CoreConnectorCredential
        {
            CoreConnectorCredentialUser = user,
            CoreConnectorCredentialPass = password, // TODO: Implementar encriptación
            CoreConnectorCredentialKey = key,
            CreateAt = DateTime.UtcNow,
            ModifyAt = DateTime.UtcNow
        };

        _context.CoreConnectorCredentials.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<CoreConnectorCredentialDto>(entity);

        // Crear evento con los campos correctos
        var createdEvent = new CoreConnectorCredentialCreatedEvent
        {
            CoreConnectorCredentialId = result.CoreConnectorCredentialId,
            CoreConnectorCredentialUser = result.CoreConnectorCredentialUser,
            CoreConnectorCredentialPass = password, // Usar el password original ya que el DTO no lo expone
            CoreConnectorCredentialKey = result.CoreConnectorCredentialKey,
            MicroserviceActive = null,
            MicroserviceDeleted = null,
            DeleteAt = null,
            CreatedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishCoreConnectorCredentialCreatedAsync(createdEvent);

        return result;
    }

    public async Task<CoreConnectorCredentialDto?> UpdateAsync(
        long id,
        string? user,
        string? password,
        string? key,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.CoreConnectorCredentials
            .FirstOrDefaultAsync(x => x.CoreConnectorCredentialId == id, cancellationToken);
        if (entity == null) return null;

        if (user != null) entity.CoreConnectorCredentialUser = user;
        if (password != null) entity.CoreConnectorCredentialPass = password; // TODO: Implementar encriptación
        if (key != null) entity.CoreConnectorCredentialKey = key;
        entity.ModifyAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<CoreConnectorCredentialDto>(entity);

        // Crear evento con los campos correctos
        var updatedEvent = new CoreConnectorCredentialUpdatedEvent
        {
            CoreConnectorCredentialId = result.CoreConnectorCredentialId,
            CoreConnectorCredentialUser = result.CoreConnectorCredentialUser,
            CoreConnectorCredentialPass = password, // Usar el password original ya que el DTO no lo expone
            CoreConnectorCredentialKey = result.CoreConnectorCredentialKey,
            MicroserviceActive = null,
            MicroserviceDeleted = null,
            DeleteAt = null,
            UpdatedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishCoreConnectorCredentialUpdatedAsync(updatedEvent);

        return result;
    }

    public async Task<bool> DeleteAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.CoreConnectorCredentials
            .FirstOrDefaultAsync(x => x.CoreConnectorCredentialId == id, cancellationToken);
        if (entity == null) return false;

        _context.CoreConnectorCredentials.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        // Crear evento con los campos correctos
        var deletedEvent = new CoreConnectorCredentialDeletedEvent
        {
            CoreConnectorCredentialId = entity.CoreConnectorCredentialId,
            CoreConnectorCredentialUser = entity.CoreConnectorCredentialUser,
            DeletedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishCoreConnectorCredentialDeletedAsync(deletedEvent);

        return true;
    }
}
