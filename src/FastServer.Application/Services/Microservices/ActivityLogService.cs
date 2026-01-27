using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Domain.Entities.Microservices;
using FastServer.Domain.Enums;
using FastServer.Domain.Interfaces;

namespace FastServer.Application.Services.Microservices;

/// <summary>
/// Servicio para gestionar logs de actividad
/// </summary>
public class ActivityLogService
{
    private readonly IDataSourceFactory _dataSourceFactory;
    private readonly IMapper _mapper;

    public ActivityLogService(IDataSourceFactory dataSourceFactory, IMapper mapper)
    {
        _dataSourceFactory = dataSourceFactory;
        _mapper = mapper;
    }

    public async Task<ActivityLogDto?> GetByIdAsync(
        Guid id,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<ActivityLog>();
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<ActivityLogDto>(entity);
    }

    public async Task<List<ActivityLogDto>> GetByUserIdAsync(
        Guid userId,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<ActivityLog>();
        var entities = await repository.FindAsync(a => a.UserId == userId, cancellationToken);
        return _mapper.Map<List<ActivityLogDto>>(entities);
    }

    public async Task<List<ActivityLogDto>> GetByEntityAsync(
        string entityName,
        Guid? entityId,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<ActivityLog>();

        var entities = entityId.HasValue
            ? await repository.FindAsync(a => a.ActivityLogEntityName == entityName && a.ActivityLogEntityId == entityId.Value, cancellationToken)
            : await repository.FindAsync(a => a.ActivityLogEntityName == entityName, cancellationToken);

        return _mapper.Map<List<ActivityLogDto>>(entities);
    }

    public async Task<ActivityLogDto> CreateAsync(
        long? eventTypeId,
        string? entityName,
        Guid? entityId,
        string? description,
        Guid? userId,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<ActivityLog>();

        var entity = new ActivityLog
        {
            ActivityLogId = Guid.NewGuid(),
            EventTypeId = eventTypeId,
            ActivityLogEntityName = entityName,
            ActivityLogEntityId = entityId,
            ActivityLogDescription = description,
            UserId = userId,
            CreateAt = DateTime.UtcNow,
            ModifyAt = DateTime.UtcNow
        };

        await repository.AddAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ActivityLogDto>(entity);
    }

    public async Task<bool> DeleteAsync(
        Guid id,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<ActivityLog>();

        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;

        await repository.DeleteAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return true;
    }
}
