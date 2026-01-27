using AutoMapper;
using FastServer.Application.DTOs;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities;
using FastServer.Domain.Enums;
using FastServer.Domain.Interfaces;
using FastServer.Domain;

namespace FastServer.Application.Services;

/// <summary>
/// Implementación del servicio de LogServicesContent
/// </summary>
public class LogServicesContentService : ILogServicesContentService
{
    private readonly IDataSourceFactory _dataSourceFactory;
    private readonly IMapper _mapper;
    private readonly DataSourceType _defaultDataSource;

    public LogServicesContentService(
        IDataSourceFactory dataSourceFactory,
        IMapper mapper,
        DataSourceSettings dataSourceSettings)
    {
        _dataSourceFactory = dataSourceFactory;
        _mapper = mapper;
        _defaultDataSource = dataSourceSettings.DefaultDataSource;
    }

    public async Task<LogServicesContentDto?> GetByIdAsync(long id, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entity = await uow.LogServicesContents.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : _mapper.Map<LogServicesContentDto>(entity);
    }

    public async Task<IEnumerable<LogServicesContentDto>> GetByLogIdAsync(long logId, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entities = await uow.LogServicesContents.GetByLogIdAsync(logId, cancellationToken);
        return _mapper.Map<IEnumerable<LogServicesContentDto>>(entities);
    }

    public async Task<IEnumerable<LogServicesContentDto>> SearchByContentAsync(string searchText, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entities = await uow.LogServicesContents.SearchByContentTextAsync(searchText, cancellationToken);
        return _mapper.Map<IEnumerable<LogServicesContentDto>>(entities);
    }

    public async Task<LogServicesContentDto> CreateAsync(CreateLogServicesContentDto dto, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entity = _mapper.Map<LogServicesContent>(dto);
        var created = await uow.LogServicesContents.AddAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        return _mapper.Map<LogServicesContentDto>(created);
    }

    public async Task<bool> DeleteAsync(long id, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entity = await uow.LogServicesContents.GetByIdAsync(id, cancellationToken);

        if (entity == null)
            return false;

        await uow.LogServicesContents.DeleteAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        return true;
    }
}
