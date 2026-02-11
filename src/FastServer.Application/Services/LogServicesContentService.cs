using AutoMapper;
using FastServer.Application.DTOs;
using FastServer.Application.EventPublishers;
using FastServer.Application.Events.LogServicesContentEvents;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastServer.Application.Services;

/// <summary>
/// Servicio de aplicación para gestionar contenidos de logs (LogServicesContent).
/// Proporciona operaciones CRUD y consultas especializadas usando PostgreSQL (BD: FastServer_Logs).
/// </summary>
public class LogServicesContentService : ILogServicesContentService
{
    private readonly ILogsDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogServicesContentEventPublisher _eventPublisher;

    /// <summary>
    /// Inicializa una nueva instancia del servicio LogServicesContentService.
    /// </summary>
    /// <param name="context">Contexto de base de datos PostgreSQL para logs (FastServer_Logs)</param>
    /// <param name="mapper">Mapeador para convertir entre entidades y DTOs</param>
    /// <param name="eventPublisher">Publisher de eventos para suscripciones GraphQL</param>
    public LogServicesContentService(
        ILogsDbContext context,
        IMapper mapper,
        ILogServicesContentEventPublisher eventPublisher)
    {
        _context = context;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<LogServicesContentDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.LogServicesContents
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.LogId == id, cancellationToken);

        return entity == null ? null : _mapper.Map<LogServicesContentDto>(entity);
    }

    public async Task<IEnumerable<LogServicesContentDto>> GetByLogIdAsync(long logId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.LogServicesContents
            .AsNoTracking()
            .Where(x => x.LogId == logId)
            .OrderBy(x => x.LogServicesDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<LogServicesContentDto>>(entities);
    }

    public async Task<IEnumerable<LogServicesContentDto>> SearchByContentAsync(string searchText, CancellationToken cancellationToken = default)
    {
        var entities = await _context.LogServicesContents
            .AsNoTracking()
            .Where(x => x.LogServicesContentText != null && x.LogServicesContentText.Contains(searchText))
            .OrderByDescending(x => x.LogServicesDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<LogServicesContentDto>>(entities);
    }

    public async Task<LogServicesContentDto> CreateAsync(CreateLogServicesContentDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<LogServicesContent>(dto);
        await _context.LogServicesContents.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<LogServicesContentDto>(entity);

        // Crear evento con los campos correctos
        var createdEvent = new LogServicesContentCreatedEvent
        {
            LogId = result.LogId,
            LogServicesDate = result.LogServicesDate,
            LogServicesLogLevel = result.LogServicesLogLevel,
            LogServicesState = result.LogServicesState,
            LogServicesContentText = result.LogServicesContentText,
            CreatedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishLogServicesContentCreatedAsync(createdEvent);

        return result;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.LogServicesContents
            .FirstOrDefaultAsync(x => x.LogId == id, cancellationToken);

        if (entity == null)
            return false;

        _context.LogServicesContents.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        // Crear evento con los campos correctos
        var deletedEvent = new LogServicesContentDeletedEvent
        {
            LogId = entity.LogId,
            LogServicesLogLevel = entity.LogServicesLogLevel,
            LogServicesState = entity.LogServicesState,
            DeletedAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishLogServicesContentDeletedAsync(deletedEvent);

        return true;
    }
}
