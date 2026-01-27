using AutoMapper;
using FastServer.Application.DTOs.Microservices;
using FastServer.Domain.Entities.Microservices;
using FastServer.Domain.Enums;
using FastServer.Domain.Interfaces;

namespace FastServer.Application.Services.Microservices;

/// <summary>
/// Servicio para gestionar tipos de eventos
/// </summary>
public class EventTypeService
{
    private readonly IDataSourceFactory _dataSourceFactory;
    private readonly IMapper _mapper;

    public EventTypeService(IDataSourceFactory dataSourceFactory, IMapper mapper)
    {
        _dataSourceFactory = dataSourceFactory;
        _mapper = mapper;
    }

    public async Task<EventTypeDto?> GetByIdAsync(
        long id,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<EventType>();
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<EventTypeDto>(entity);
    }

    public async Task<List<EventTypeDto>> GetAllAsync(
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<EventType>();
        var entities = await repository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<EventTypeDto>>(entities);
    }

    public async Task<EventTypeDto> CreateAsync(
        string description,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<EventType>();

        var entity = new EventType
        {
            EventTypeDescription = description,
            CreateAt = DateTime.UtcNow,
            ModifyAt = DateTime.UtcNow
        };

        await repository.AddAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return _mapper.Map<EventTypeDto>(entity);
    }

    public async Task<EventTypeDto?> UpdateAsync(
        long id,
        string description,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<EventType>();

        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return null;

        entity.EventTypeDescription = description;
        entity.ModifyAt = DateTime.UtcNow;

        await repository.UpdateAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return _mapper.Map<EventTypeDto>(entity);
    }

    public async Task<bool> DeleteAsync(
        long id,
        DataSourceType dataSourceType,
        CancellationToken cancellationToken = default)
    {
        using var uow = _dataSourceFactory.CreateUnitOfWork(dataSourceType);
        var repository = uow.GetRepository<EventType>();

        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;

        await repository.DeleteAsync(entity, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return true;
    }
}
