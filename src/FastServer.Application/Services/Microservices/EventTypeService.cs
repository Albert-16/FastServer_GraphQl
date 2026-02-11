using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Services.Microservices;

/// <summary>
/// Servicio para gestionar tipos de eventos en PostgreSQL (BD: FastServer)
/// </summary>
public class EventTypeService
{
    private readonly IMicroservicesDbContext _context;
    private readonly IMapper _mapper;

    public EventTypeService(IMicroservicesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EventTypeDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.EventTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.EventTypeId == id, cancellationToken);
        return entity == null ? null : _mapper.Map<EventTypeDto>(entity);
    }

    public async Task<List<EventTypeDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.EventTypes
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<EventTypeDto>>(entities);
    }

    public async Task<EventTypeDto> CreateAsync(
        string description,
        CancellationToken cancellationToken = default)
    {
        var entity = new EventType
        {
            EventTypeDescription = description,
            CreateAt = DateTime.UtcNow,
            ModifyAt = DateTime.UtcNow
        };

        _context.EventTypes.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<EventTypeDto>(entity);
    }

    public async Task<EventTypeDto?> UpdateAsync(
        long id,
        string description,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.EventTypes
            .FirstOrDefaultAsync(x => x.EventTypeId == id, cancellationToken);
        if (entity == null) return null;

        entity.EventTypeDescription = description;
        entity.ModifyAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<EventTypeDto>(entity);
    }

    public async Task<bool> DeleteAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.EventTypes
            .FirstOrDefaultAsync(x => x.EventTypeId == id, cancellationToken);
        if (entity == null) return false;

        _context.EventTypes.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
