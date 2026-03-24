using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Application.Interfaces;
using FastServer.Application.Interfaces.Microservices;
using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Services.Microservices;

/// <summary>
/// Servicio para gestionar nodos (tabla intermedia Method-Cluster) en PostgreSQL (BD: FastServer)
/// </summary>
public class NodoService : INodoService
{
    private readonly IMicroservicesDbContext _context;
    private readonly IMapper _mapper;

    public NodoService(IMicroservicesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<NodoDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Nodos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.NodoId == id, cancellationToken);
        return entity == null ? null : _mapper.Map<NodoDto>(entity);
    }

    public async Task<IEnumerable<NodoDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.Nodos
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<NodoDto>>(entities);
    }

    public async Task<IEnumerable<NodoDto>> GetByMethodIdAsync(
        Guid methodId,
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.Nodos
            .AsNoTracking()
            .Where(n => n.MicroserviceMethodId == methodId)
            .ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<NodoDto>>(entities);
    }

    public async Task<IEnumerable<NodoDto>> GetByClusterIdAsync(
        Guid clusterId,
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.Nodos
            .AsNoTracking()
            .Where(n => n.MicroservicesClusterId == clusterId)
            .ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<NodoDto>>(entities);
    }

    public async Task<NodoDto> CreateAsync(
        Guid methodId,
        Guid clusterId,
        CancellationToken cancellationToken = default)
    {
        var entity = new Nodo
        {
            NodoId = Guid.CreateVersion7(),
            MicroserviceMethodId = methodId,
            MicroservicesClusterId = clusterId,
            CreateAt = DateTime.UtcNow,
            ModifyAt = DateTime.UtcNow
        };

        _context.Nodos.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<NodoDto>(entity);
    }

    public async Task<NodoDto?> UpdateAsync(
        Guid id,
        Guid? methodId,
        Guid? clusterId,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Nodos
            .FirstOrDefaultAsync(x => x.NodoId == id, cancellationToken);
        if (entity == null) return null;

        if (methodId.HasValue) entity.MicroserviceMethodId = methodId.Value;
        if (clusterId.HasValue) entity.MicroservicesClusterId = clusterId.Value;
        entity.ModifyAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<NodoDto>(entity);
    }

    public async Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Nodos
            .FirstOrDefaultAsync(x => x.NodoId == id, cancellationToken);
        if (entity == null) return false;

        _context.Nodos.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
