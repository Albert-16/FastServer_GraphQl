using FastServer.Application.DTOs.Microservices;
using FastServer.Application.Interfaces;
using FastServer.Application.Services.Microservices;
using FastServer.Domain.Entities.Microservices;
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

    [GraphQLDescription("Obtiene todos los microservicios desde FastServer (PostgreSQL)")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<MicroserviceRegister> GetAllMicroservices(
        [Service] IMicroservicesDbContext context)
    {
        return context.MicroserviceRegisters;
    }

    [GraphQLDescription("Obtiene los microservicios activos desde FastServer (PostgreSQL)")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<MicroserviceRegister> GetActiveMicroservices(
        [Service] IMicroservicesDbContext context)
    {
        return context.MicroserviceRegisters
            .Where(m => m.MicroserviceActive == true && m.MicroserviceDeleted != true);
    }

    [GraphQLDescription("Obtiene microservicios por ID de cluster desde FastServer (PostgreSQL)")]
    [UseProjection]
    public IQueryable<MicroserviceRegister> GetMicroservicesByClusterId(
        [Service] IMicroservicesDbContext context,
        long clusterId)
    {
        return context.MicroserviceRegisters
            .Where(m => m.MicroserviceClusterId == clusterId);
    }

    // ========================================
    // MICROSERVICES CLUSTERS
    // ========================================

    [GraphQLDescription("Obtiene todos los clusters de microservicios desde FastServer (PostgreSQL)")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<MicroservicesCluster> GetAllClusters(
        [Service] IMicroservicesDbContext context)
    {
        return context.MicroservicesClusters;
    }

    [GraphQLDescription("Obtiene los clusters activos desde FastServer (PostgreSQL)")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<MicroservicesCluster> GetActiveClusters(
        [Service] IMicroservicesDbContext context)
    {
        return context.MicroservicesClusters
            .Where(c => c.MicroservicesClusterActive == true && c.MicroservicesClusterDeleted != true);
    }

    // ========================================
    // USERS
    // ========================================

    [GraphQLDescription("Obtiene todos los usuarios desde FastServer (PostgreSQL)")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<User> GetAllUsers(
        [Service] IMicroservicesDbContext context)
    {
        return context.Users;
    }

    [GraphQLDescription("Obtiene los usuarios activos desde FastServer (PostgreSQL)")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<User> GetActiveUsers(
        [Service] IMicroservicesDbContext context)
    {
        return context.Users
            .Where(u => u.UserActive == true);
    }

    // ========================================
    // ACTIVITY LOGS
    // ========================================

    [GraphQLDescription("Obtiene todos los logs de actividad desde FastServer (PostgreSQL)")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ActivityLog> GetAllActivityLogs(
        [Service] IMicroservicesDbContext context)
    {
        return context.ActivityLogs;
    }

    [GraphQLDescription("Obtiene los logs de actividad por usuario desde FastServer (PostgreSQL)")]
    [UseProjection]
    public IQueryable<ActivityLog> GetActivityLogsByUser(
        [Service] IMicroservicesDbContext context,
        Guid userId)
    {
        return context.ActivityLogs
            .Where(a => a.UserId == userId);
    }

    [GraphQLDescription("Obtiene los logs de actividad por entidad desde FastServer (PostgreSQL)")]
    [UseProjection]
    public IQueryable<ActivityLog> GetActivityLogsByEntity(
        [Service] IMicroservicesDbContext context,
        string entityName,
        Guid? entityId = null)
    {
        var query = context.ActivityLogs
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

    [GraphQLDescription("Obtiene todos los tipos de eventos desde FastServer (PostgreSQL)")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<EventType> GetAllEventTypes(
        [Service] IMicroservicesDbContext context)
    {
        return context.EventTypes;
    }

    // ========================================
    // CORE CONNECTOR CREDENTIALS
    // ========================================

    [GraphQLDescription("Obtiene todas las credenciales de conectores desde FastServer (PostgreSQL)")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<CoreConnectorCredential> GetAllCredentials(
        [Service] IMicroservicesDbContext context)
    {
        return context.CoreConnectorCredentials;
    }

    // ========================================
    // MICROSERVICE CORE CONNECTORS
    // ========================================

    [GraphQLDescription("Obtiene todos los conectores de microservicios desde FastServer (PostgreSQL)")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<MicroserviceCoreConnector> GetAllConnectors(
        [Service] IMicroservicesDbContext context)
    {
        return context.MicroserviceCoreConnectors;
    }

    [GraphQLDescription("Obtiene conectores por ID de microservicio desde FastServer (PostgreSQL)")]
    [UseProjection]
    public IQueryable<MicroserviceCoreConnector> GetConnectorsByMicroserviceId(
        [Service] IMicroservicesDbContext context,
        long microserviceId)
    {
        return context.MicroserviceCoreConnectors
            .Where(c => c.MicroserviceId == microserviceId);
    }
}
