using AutoMapper;
using FastServer.Application.DTOs;
using FastServer.Application.Interfaces;
using FastServer.Domain.Entities;
using FastServer.Domain.Enums;
using FastServer.Domain.Interfaces;

namespace FastServer.Application.Services;

/// <summary>
/// Implementación del servicio de LogServicesHeader
/// </summary>
public class LogServicesHeaderService : ILogServicesHeaderService
{
    private readonly IDataSourceFactory _dataSourceFactory;
    private readonly IMapper _mapper;
    private readonly DataSourceType _defaultDataSource;

    public LogServicesHeaderService(
        IDataSourceFactory dataSourceFactory,
        IMapper mapper,
        DataSourceType defaultDataSource = DataSourceType.PostgreSQL)
    {
        _dataSourceFactory = dataSourceFactory;
        _mapper = mapper;
        _defaultDataSource = defaultDataSource;
    }

    public async Task<LogServicesHeaderDto?> GetByIdAsync(long id, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entity = await uow.LogServicesHeaders.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : _mapper.Map<LogServicesHeaderDto>(entity);
    }

    public async Task<LogServicesHeaderDto?> GetWithDetailsAsync(long id, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entity = await uow.LogServicesHeaders.GetWithDetailsAsync(id, cancellationToken);
        return entity == null ? null : _mapper.Map<LogServicesHeaderDto>(entity);
    }

    public async Task<PaginatedResultDto<LogServicesHeaderDto>> GetAllAsync(PaginationParamsDto pagination, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var totalCount = await uow.LogServicesHeaders.CountAsync(cancellationToken);
        var entities = await uow.LogServicesHeaders.GetAllAsync(cancellationToken);

        var pagedEntities = entities
            .Skip(pagination.Skip)
            .Take(pagination.PageSize)
            .ToList();

        return new PaginatedResultDto<LogServicesHeaderDto>
        {
            Items = _mapper.Map<IEnumerable<LogServicesHeaderDto>>(pagedEntities),
            TotalCount = totalCount,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize
        };
    }

    public async Task<PaginatedResultDto<LogServicesHeaderDto>> GetByFilterAsync(LogFilterDto filter, PaginationParamsDto pagination, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(filter.DataSource ?? _defaultDataSource);

        var query = uow.LogServicesHeaders.Query();

        if (filter.StartDate.HasValue)
            query = query.Where(x => x.LogDateIn >= filter.StartDate.Value);

        if (filter.EndDate.HasValue)
            query = query.Where(x => x.LogDateIn <= filter.EndDate.Value);

        if (filter.State.HasValue)
            query = query.Where(x => x.LogState == filter.State.Value);

        if (!string.IsNullOrEmpty(filter.MicroserviceName))
            query = query.Where(x => x.MicroserviceName != null && x.MicroserviceName.Contains(filter.MicroserviceName));

        if (!string.IsNullOrEmpty(filter.UserId))
            query = query.Where(x => x.UserId == filter.UserId);

        if (!string.IsNullOrEmpty(filter.TransactionId))
            query = query.Where(x => x.TransactionId == filter.TransactionId);

        if (!string.IsNullOrEmpty(filter.HttpMethod))
            query = query.Where(x => x.HttpMethod == filter.HttpMethod);

        if (filter.HasErrors.HasValue && filter.HasErrors.Value)
            query = query.Where(x => x.ErrorCode != null);

        var totalCount = query.Count();
        var entities = query
            .OrderByDescending(x => x.LogDateIn)
            .Skip(pagination.Skip)
            .Take(pagination.PageSize)
            .ToList();

        return new PaginatedResultDto<LogServicesHeaderDto>
        {
            Items = _mapper.Map<IEnumerable<LogServicesHeaderDto>>(entities),
            TotalCount = totalCount,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize
        };
    }

    public async Task<LogServicesHeaderDto> CreateAsync(CreateLogServicesHeaderDto dto, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entity = _mapper.Map<LogServicesHeader>(dto);
        var created = await uow.LogServicesHeaders.AddAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        return _mapper.Map<LogServicesHeaderDto>(created);
    }

    public async Task<LogServicesHeaderDto> UpdateAsync(UpdateLogServicesHeaderDto dto, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entity = await uow.LogServicesHeaders.GetByIdAsync(dto.LogId, cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException($"LogServicesHeader with id {dto.LogId} not found");

        if (dto.LogDateOut.HasValue)
            entity.LogDateOut = dto.LogDateOut.Value;
        if (dto.LogState.HasValue)
            entity.LogState = dto.LogState.Value;
        if (dto.ErrorCode != null)
            entity.ErrorCode = dto.ErrorCode;
        if (dto.ErrorDescription != null)
            entity.ErrorDescription = dto.ErrorDescription;
        if (dto.RequestDuration.HasValue)
            entity.RequestDuration = dto.RequestDuration.Value;

        await uow.LogServicesHeaders.UpdateAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LogServicesHeaderDto>(entity);
    }

    public async Task<bool> DeleteAsync(long id, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entity = await uow.LogServicesHeaders.GetByIdAsync(id, cancellationToken);

        if (entity == null)
            return false;

        await uow.LogServicesHeaders.DeleteAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IEnumerable<LogServicesHeaderDto>> GetFailedLogsAsync(DateTime? fromDate = null, DataSourceType? dataSource = null, CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSource ?? _defaultDataSource);
        var entities = await uow.LogServicesHeaders.GetFailedLogsAsync(fromDate, cancellationToken);
        return _mapper.Map<IEnumerable<LogServicesHeaderDto>>(entities);
    }
}
