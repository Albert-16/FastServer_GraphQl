using AutoMapper;
using FastServer.Application.DTOs;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities;
using FastServer.Domain.Enums;
using FastServer.Domain.Interfaces;

namespace FastServer.Application.Services;

/// <summary>
/// Implementación del servicio de LogMicroservice
/// </summary>
public class LogMicroserviceService : ILogMicroserviceService
{
    private readonly IDataSourceFactory _dataSourceFactory;
    private readonly IMapper _mapper;
    private readonly DataSourceType _defaultDataSource;

    public LogMicroserviceService(
        IDataSourceFactory dataSourceFactory,
        IMapper mapper,
        DataSourceType defaultDataSource = DataSourceType.PostgreSQL)
    {
        _dataSourceFactory = dataSourceFactory;
        _mapper = mapper;
        _defaultDataSource = defaultDataSource;
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
        return _mapper.Map<LogMicroserviceDto>(created);
    }

    public async Task<bool> DeleteAsync(long id, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entity = await uow.LogMicroservices.GetByIdAsync(id, cancellationToken);

        if (entity == null)
            return false;

        await uow.LogMicroservices.DeleteAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        return true;
    }
}
