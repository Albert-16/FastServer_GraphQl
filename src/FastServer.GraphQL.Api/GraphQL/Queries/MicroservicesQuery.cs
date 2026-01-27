using FastServer.Application.DTOs.Microservices;
using FastServer.Application.Services.Microservices;
using FastServer.Domain.Entities.Microservices;
using FastServer.Domain.Enums;
using FastServer.Domain.Interfaces;
using FastServer.GraphQL.Api.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Queries;

[ExtendObjectType<Query>]
public class MicroservicesQuery
{
    // ========================================
    // MICROSERVICE REGISTERS
    // ========================================

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<MicroserviceRegister> GetAllMicroservices(
        [Service] IDataSourceFactory factory,
        DataSourceType dataSource = DataSourceType.SqlServer)
    {
        var uow = factory.CreateUnitOfWork(dataSource);
        return uow.GetRepository<MicroserviceRegister>().Query();
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<MicroserviceRegister> GetActiveMicroservices(
        [Service] IDataSourceFactory factory,
        DataSourceType dataSource = DataSourceType.SqlServer)
    {
        var uow = factory.CreateUnitOfWork(dataSource);
        return uow.GetRepository<MicroserviceRegister>()
            .Query()
            .Where(m => m.MicroserviceActive == true && m.MicroserviceDeleted != true);
    }

    [UseProjection]
    public IQueryable<MicroserviceRegister> GetMicroservicesByClusterId(
        [Service] IDataSourceFactory factory,
        long clusterId,
        DataSourceType dataSource = DataSourceType.SqlServer)
    {
        var uow = factory.CreateUnitOfWork(dataSource);
        return uow.GetRepository<MicroserviceRegister>()
            .Query()
            .Where(m => m.MicroserviceClusterId == clusterId);
    }

    // ========================================
    // MICROSERVICES CLUSTERS
    // ========================================

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<MicroservicesCluster> GetAllClusters(
        [Service] IDataSourceFactory factory,
        DataSourceType dataSource = DataSourceType.SqlServer)
    {
        var uow = factory.CreateUnitOfWork(dataSource);
        return uow.GetRepository<MicroservicesCluster>().Query();
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<MicroservicesCluster> GetActiveClusters(
        [Service] IDataSourceFactory factory,
        DataSourceType dataSource = DataSourceType.SqlServer)
    {
        var uow = factory.CreateUnitOfWork(dataSource);
        return uow.GetRepository<MicroservicesCluster>()
            .Query()
            .Where(c => c.MicroservicesClusterActive == true && c.MicroservicesClusterDeleted != true);
    }

    // ========================================
    // USERS
    // ========================================

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<User> GetAllUsers(
        [Service] IDataSourceFactory factory,
        DataSourceType dataSource = DataSourceType.SqlServer)
    {
        var uow = factory.CreateUnitOfWork(dataSource);
        return uow.GetRepository<User>().Query();
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<User> GetActiveUsers(
        [Service] IDataSourceFactory factory,
        DataSourceType dataSource = DataSourceType.SqlServer)
    {
        var uow = factory.CreateUnitOfWork(dataSource);
        return uow.GetRepository<User>()
            .Query()
            .Where(u => u.UserActive == true);
    }

    // ========================================
    // ACTIVITY LOGS
    // ========================================

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ActivityLog> GetAllActivityLogs(
        [Service] IDataSourceFactory factory,
        DataSourceType dataSource = DataSourceType.SqlServer)
    {
        var uow = factory.CreateUnitOfWork(dataSource);
        return uow.GetRepository<ActivityLog>().Query();
    }

    [UseProjection]
    public IQueryable<ActivityLog> GetActivityLogsByUser(
        [Service] IDataSourceFactory factory,
        Guid userId,
        DataSourceType dataSource = DataSourceType.SqlServer)
    {
        var uow = factory.CreateUnitOfWork(dataSource);
        return uow.GetRepository<ActivityLog>()
            .Query()
            .Where(a => a.UserId == userId);
    }

    [UseProjection]
    public IQueryable<ActivityLog> GetActivityLogsByEntity(
        [Service] IDataSourceFactory factory,
        string entityName,
        Guid? entityId = null,
        DataSourceType dataSource = DataSourceType.SqlServer)
    {
        var uow = factory.CreateUnitOfWork(dataSource);
        var query = uow.GetRepository<ActivityLog>()
            .Query()
            .Where(a => a.ActivityLogEntityName == entityName);

        if (entityId.HasValue)
        {
            query = query.Where(a => a.ActivityLogEntityId == entityId.Value);
        }

        return query;
    }

    // ========================================
    // EVENT TYPES
    // ========================================

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<EventType> GetAllEventTypes(
        [Service] IDataSourceFactory factory,
        DataSourceType dataSource = DataSourceType.SqlServer)
    {
        var uow = factory.CreateUnitOfWork(dataSource);
        return uow.GetRepository<EventType>().Query();
    }

    // ========================================
    // CORE CONNECTOR CREDENTIALS
    // ========================================

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<CoreConnectorCredential> GetAllCredentials(
        [Service] IDataSourceFactory factory,
        DataSourceType dataSource = DataSourceType.SqlServer)
    {
        var uow = factory.CreateUnitOfWork(dataSource);
        return uow.GetRepository<CoreConnectorCredential>().Query();
    }

    // ========================================
    // MICROSERVICE CORE CONNECTORS
    // ========================================

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<MicroserviceCoreConnector> GetAllConnectors(
        [Service] IDataSourceFactory factory,
        DataSourceType dataSource = DataSourceType.SqlServer)
    {
        var uow = factory.CreateUnitOfWork(dataSource);
        return uow.GetRepository<MicroserviceCoreConnector>().Query();
    }

    [UseProjection]
    public IQueryable<MicroserviceCoreConnector> GetConnectorsByMicroserviceId(
        [Service] IDataSourceFactory factory,
        long microserviceId,
        DataSourceType dataSource = DataSourceType.SqlServer)
    {
        var uow = factory.CreateUnitOfWork(dataSource);
        return uow.GetRepository<MicroserviceCoreConnector>()
            .Query()
            .Where(c => c.MicroserviceId == microserviceId);
    }
}
