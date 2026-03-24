using AutoMapper;
using FastServer.Application.DTOs;
using FastServer.Application.DTOs.Microservices;
using FastServer.Application.EventPublishers;
using FastServer.Application.Events.MicroserviceRegisterEvents;
using FastServer.Application.Interfaces;
using FastServer.Application.Interfaces.Microservices;
using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Services.Microservices;

/// <summary>
/// Servicio para gestionar registros de microservicios en PostgreSQL (BD: FastServer)
/// </summary>
public class MicroserviceRegisterService : IMicroserviceRegisterService
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
        Guid id,
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

    public async Task<PaginatedResultDto<MicroserviceRegisterDto>> GetAllPaginatedAsync(
        PaginationParamsDto pagination, CancellationToken ct = default)
    {
        int totalCount = await _context.MicroserviceRegisters.CountAsync(ct);
        var items = await _context.MicroserviceRegisters
            .AsNoTracking()
            .Include(x => x.MicroserviceType)
            .OrderByDescending(x => x.CreateAt)
            .Skip(pagination.Skip)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return new PaginatedResultDto<MicroserviceRegisterDto>
        {
            Items = _mapper.Map<IEnumerable<MicroserviceRegisterDto>>(items),
            TotalCount = totalCount,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize
        };
    }

    public async Task<MicroserviceRegisterDto> CreateAsync(
        string? name,
        bool active,
        bool coreConnection,
        string? soapBase,
        Guid? microserviceTypeId,
        CancellationToken cancellationToken = default)
    {
        var entity = new MicroserviceRegister
        {
            MicroserviceId = Guid.CreateVersion7(),
            MicroserviceName = name,
            MicroserviceActive = active,
            MicroserviceDeleted = false,
            MicroserviceCoreConnection = coreConnection,
            SoapBase = soapBase,
            MicroserviceTypeId = microserviceTypeId,
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
            MicroserviceName = result.MicroserviceName,
            MicroserviceActive = result.MicroserviceActive,
            MicroserviceDeleted = result.MicroserviceDeleted,
            MicroserviceCoreConnection = result.MicroserviceCoreConnection,
            SoapBase = result.SoapBase,
            MicroserviceTypeId = result.MicroserviceTypeId,
            DeleteAt = result.DeleteAt,
            CreatedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishMicroserviceRegisterCreatedAsync(createdEvent);

        return result;
    }

    public async Task<MicroserviceRegisterDto?> UpdateAsync(
        Guid id,
        string? name,
        bool? active,
        bool? coreConnection,
        string? soapBase,
        Guid? microserviceTypeId,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.MicroserviceRegisters
            .FirstOrDefaultAsync(x => x.MicroserviceId == id, cancellationToken);
        if (entity == null) return null;

        if (name != null) entity.MicroserviceName = name;
        if (active.HasValue) entity.MicroserviceActive = active.Value;
        if (coreConnection.HasValue) entity.MicroserviceCoreConnection = coreConnection.Value;
        if (soapBase != null) entity.SoapBase = soapBase;
        if (microserviceTypeId.HasValue) entity.MicroserviceTypeId = microserviceTypeId;
        entity.ModifyAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<MicroserviceRegisterDto>(entity);

        // Crear evento con los campos correctos
        var updatedEvent = new MicroserviceRegisterUpdatedEvent
        {
            MicroserviceId = result.MicroserviceId,
            MicroserviceName = result.MicroserviceName,
            MicroserviceActive = result.MicroserviceActive,
            MicroserviceDeleted = result.MicroserviceDeleted,
            MicroserviceCoreConnection = result.MicroserviceCoreConnection,
            SoapBase = result.SoapBase,
            MicroserviceTypeId = result.MicroserviceTypeId,
            DeleteAt = result.DeleteAt,
            UpdatedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishMicroserviceRegisterUpdatedAsync(updatedEvent);

        return result;
    }

    public async Task<bool> SoftDeleteAsync(
        Guid id,
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
        Guid id,
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
