using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Services.Microservices;

/// <summary>
/// Servicio para gestionar conectores entre microservicios y el core en PostgreSQL (BD: FastServer)
/// </summary>
public class MicroserviceCoreConnectorService
{
    private readonly IMicroservicesDbContext _context;
    private readonly IMapper _mapper;

    public MicroserviceCoreConnectorService(IMicroservicesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MicroserviceCoreConnectorDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.MicroserviceCoreConnectors
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MicroserviceCoreConnectorId == id, cancellationToken);
        return entity == null ? null : _mapper.Map<MicroserviceCoreConnectorDto>(entity);
    }

    public async Task<List<MicroserviceCoreConnectorDto>> GetByMicroserviceIdAsync(
        long microserviceId,
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.MicroserviceCoreConnectors
            .AsNoTracking()
            .Where(c => c.MicroserviceId == microserviceId)
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<MicroserviceCoreConnectorDto>>(entities);
    }

    public async Task<MicroserviceCoreConnectorDto> CreateAsync(
        long? credentialId,
        long? microserviceId,
        CancellationToken cancellationToken = default)
    {
        var entity = new MicroserviceCoreConnector
        {
            CoreConnectorCredentialId = credentialId,
            MicroserviceId = microserviceId,
            CreateAt = DateTime.UtcNow,
            ModifyAt = DateTime.UtcNow
        };

        _context.MicroserviceCoreConnectors.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<MicroserviceCoreConnectorDto>(entity);
    }

    public async Task<MicroserviceCoreConnectorDto?> UpdateAsync(
        long id,
        long? credentialId,
        long? microserviceId,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.MicroserviceCoreConnectors
            .FirstOrDefaultAsync(x => x.MicroserviceCoreConnectorId == id, cancellationToken);
        if (entity == null) return null;

        if (credentialId.HasValue) entity.CoreConnectorCredentialId = credentialId;
        if (microserviceId.HasValue) entity.MicroserviceId = microserviceId;
        entity.ModifyAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<MicroserviceCoreConnectorDto>(entity);
    }

    public async Task<bool> DeleteAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.MicroserviceCoreConnectors
            .FirstOrDefaultAsync(x => x.MicroserviceCoreConnectorId == id, cancellationToken);
        if (entity == null) return false;

        _context.MicroserviceCoreConnectors.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
