using AutoMapper;
using FastServer.Application.DTOs;
using FastServer.Application.EventPublishers;
using FastServer.Application.Events.LogMicroserviceEvents;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Services;

/// <summary>
/// Servicio de aplicación para gestionar logs de microservicios (LogMicroservice).
/// Proporciona operaciones CRUD y consultas especializadas usando PostgreSQL (BD: FastServer_Logs).
/// </summary>
public class LogMicroserviceService : ILogMicroserviceService
{
    private readonly ILogsDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogMicroserviceEventPublisher _eventPublisher;

    /// <summary>
    /// Inicializa una nueva instancia del servicio LogMicroserviceService.
    /// </summary>
    /// <param name="context">Contexto de base de datos PostgreSQL para logs (FastServer_Logs)</param>
    /// <param name="mapper">Mapeador para convertir entre entidades y DTOs</param>
    /// <param name="eventPublisher">Publisher de eventos para suscripciones GraphQL</param>
    public LogMicroserviceService(
        ILogsDbContext context,
        IMapper mapper,
        ILogMicroserviceEventPublisher eventPublisher)
    {
        _context = context;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<LogMicroserviceDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.LogMicroservices
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.LogId == id, cancellationToken);

        return entity == null ? null : _mapper.Map<LogMicroserviceDto>(entity);
    }

    public async Task<IEnumerable<LogMicroserviceDto>> GetByLogIdAsync(long logId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.LogMicroservices
            .AsNoTracking()
            .Where(x => x.LogId == logId)
            .OrderBy(x => x.LogDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<LogMicroserviceDto>>(entities);
    }

    public async Task<IEnumerable<LogMicroserviceDto>> SearchByTextAsync(string searchText, CancellationToken cancellationToken = default)
    {
        var entities = await _context.LogMicroservices
            .AsNoTracking()
            .Where(x => x.LogMicroserviceText != null && x.LogMicroserviceText.Contains(searchText))
            .OrderByDescending(x => x.LogDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<LogMicroserviceDto>>(entities);
    }

    public async Task<LogMicroserviceDto> CreateAsync(CreateLogMicroserviceDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<LogMicroservice>(dto);
        await _context.LogMicroservices.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<LogMicroserviceDto>(entity);

        await _eventPublisher.PublishLogMicroserviceCreatedAsync(new LogMicroserviceCreatedEvent
        {
            LogId = result.LogId,
            LogDate = result.LogDate,
            LogLevel = result.LogLevel,
            LogMicroserviceText = result.LogMicroserviceText,
            CreatedAt = DateTime.UtcNow
        });

        return result;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.LogMicroservices
            .FirstOrDefaultAsync(x => x.LogId == id, cancellationToken);

        if (entity == null)
            return false;

        _context.LogMicroservices.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        await _eventPublisher.PublishLogMicroserviceDeletedAsync(new LogMicroserviceDeletedEvent
        {
            LogId = id,
            DeletedAt = DateTime.UtcNow
        });

        return true;
    }
}
