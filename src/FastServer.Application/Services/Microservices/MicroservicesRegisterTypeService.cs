using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Application.Interfaces;
using FastServer.Application.Interfaces.Microservices;
using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Services.Microservices;

/// <summary>
/// Servicio para gestionar tipos de registro de microservicios en PostgreSQL (BD: FastServer)
/// </summary>
public class MicroservicesRegisterTypeService : IMicroservicesRegisterTypeService
{
    private readonly IMicroservicesDbContext _context;
    private readonly IMapper _mapper;

    public MicroservicesRegisterTypeService(IMicroservicesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MicroservicesRegisterTypeDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.MicroservicesRegisterTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MicroservicesRegisterTypeId == id, cancellationToken);
        return entity == null ? null : _mapper.Map<MicroservicesRegisterTypeDto>(entity);
    }

    public async Task<IEnumerable<MicroservicesRegisterTypeDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.MicroservicesRegisterTypes
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<MicroservicesRegisterTypeDto>>(entities);
    }

    public async Task<MicroservicesRegisterTypeDto> CreateAsync(
        string? name,
        string? description,
        CancellationToken cancellationToken = default)
    {
        var entity = new MicroservicesRegisterType
        {
            MicroservicesRegisterTypeId = Guid.CreateVersion7(),
            MicroservicesRegisterTypeName = name,
            MicroservicesRegisterTypeDescription = description,
            CreateAt = DateTime.UtcNow,
            ModifyAt = DateTime.UtcNow
        };

        _context.MicroservicesRegisterTypes.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<MicroservicesRegisterTypeDto>(entity);
    }

    public async Task<MicroservicesRegisterTypeDto?> UpdateAsync(
        Guid id,
        string? name = null,
        string? description = null,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.MicroservicesRegisterTypes
            .FirstOrDefaultAsync(x => x.MicroservicesRegisterTypeId == id, cancellationToken);
        if (entity == null) return null;

        if (name != null) entity.MicroservicesRegisterTypeName = name;
        if (description != null) entity.MicroservicesRegisterTypeDescription = description;
        entity.ModifyAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<MicroservicesRegisterTypeDto>(entity);
    }

    public async Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.MicroservicesRegisterTypes
            .FirstOrDefaultAsync(x => x.MicroservicesRegisterTypeId == id, cancellationToken);
        if (entity == null) return false;

        _context.MicroservicesRegisterTypes.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
