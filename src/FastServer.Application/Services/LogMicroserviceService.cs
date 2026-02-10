using AutoMapper;
using FastServer.Application.DTOs;
using FastServer.Application.EventPublishers;
using FastServer.Application.Events.LogMicroserviceEvents;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities;
using FastServer.Domain.Enums;
using FastServer.Domain.Interfaces;
using FastServer.Domain;

namespace FastServer.Application.Services;

/// <summary>
/// Implementación del servicio de LogMicroservice
/// </summary>
public class LogMicroserviceService : ILogMicroserviceService
{
    private readonly IDataSourceFactory _dataSourceFactory;
    private readonly IMapper _mapper;
    private readonly DataSourceType _defaultDataSource;
    private readonly ILogMicroserviceEventPublisher _eventPublisher;

    public LogMicroserviceService(
        IDataSourceFactory dataSourceFactory,
        IMapper mapper,
        DataSourceSettings dataSourceSettings,
        ILogMicroserviceEventPublisher eventPublisher)
    {
        _dataSourceFactory = dataSourceFactory;
        _mapper = mapper;
        _defaultDataSource = dataSourceSettings.DefaultDataSource;
        _eventPublisher = eventPublisher;
    }

    public async Task<LogMicroserviceDto?> GetByIdAsync(long id, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entity = await uow.LogMicroservices.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : _mapper.Map<LogMicroserviceDto>(entity);
    }

    public async Task<IEnumerable<LogMicroserviceDto>> GetByLogIdAsync(long logId, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entities = await uow.LogMicroservices.GetByLogIdAsync(logId, cancellationToken);
        return _mapper.Map<IEnumerable<LogMicroserviceDto>>(entities);
    }

    public async Task<IEnumerable<LogMicroserviceDto>> SearchByTextAsync(string searchText, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entities = await uow.LogMicroservices.SearchByTextAsync(searchText, cancellationToken);
        return _mapper.Map<IEnumerable<LogMicroserviceDto>>(entities);
    }

    public async Task<LogMicroserviceDto> CreateAsync(CreateLogMicroserviceDto dto, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entity = _mapper.Map<LogMicroservice>(dto);
        var created = await uow.LogMicroservices.AddAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<LogMicroserviceDto>(created);

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

    public async Task<bool> DeleteAsync(long id, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entity = await uow.LogMicroservices.GetByIdAsync(id, cancellationToken);

        if (entity == null)
            return false;

        await uow.LogMicroservices.DeleteAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        await _eventPublisher.PublishLogMicroserviceDeletedAsync(new LogMicroserviceDeletedEvent
        {
            LogId = id,
            DeletedAt = DateTime.UtcNow
        });

        return true;
    }
}
