using FastServer.Application.DTOs.Microservices;
using FastServer.Application.Services.Microservices;
using FastServer.Domain.Enums;
using FastServer.GraphQL.Api.GraphQL.Mutations;
using HotChocolate;
using HotChocolate.Types;

namespace FastServer.GraphQL.Api.GraphQL.Mutations;

[ExtendObjectType<Mutation>]
public class MicroservicesMutation
{
    // ========================================
    // MICROSERVICE REGISTERS - MUTATIONS
    // ========================================

    public async Task<MicroserviceRegisterDto> CreateMicroserviceAsync(
        [Service] MicroserviceRegisterService service,
        long? clusterId,
        string? name,
        bool active = true,
        bool coreConnection = false,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(clusterId, name, active, coreConnection, dataSource, cancellationToken);
    }

    public async Task<MicroserviceRegisterDto?> UpdateMicroserviceAsync(
        [Service] MicroserviceRegisterService service,
        long id,
        long? clusterId = null,
        string? name = null,
        bool? active = null,
        bool? coreConnection = null,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, clusterId, name, active, coreConnection, dataSource, cancellationToken);
    }

    public async Task<bool> SoftDeleteMicroserviceAsync(
        [Service] MicroserviceRegisterService service,
        long id,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.SoftDeleteAsync(id, dataSource, cancellationToken);
    }

    public async Task<bool> SetMicroserviceActiveAsync(
        [Service] MicroserviceRegisterService service,
        long id,
        bool active,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.SetActiveAsync(id, active, dataSource, cancellationToken);
    }

    // ========================================
    // CLUSTERS - MUTATIONS
    // ========================================

    public async Task<MicroservicesClusterDto> CreateClusterAsync(
        [Service] MicroservicesClusterService service,
        string? name,
        string? serverName = null,
        string? serverIp = null,
        bool active = true,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(name, serverName, serverIp, active, dataSource, cancellationToken);
    }

    public async Task<MicroservicesClusterDto?> UpdateClusterAsync(
        [Service] MicroservicesClusterService service,
        long id,
        string? name = null,
        string? serverName = null,
        string? serverIp = null,
        bool? active = null,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, name, serverName, serverIp, active, dataSource, cancellationToken);
    }

    public async Task<bool> SoftDeleteClusterAsync(
        [Service] MicroservicesClusterService service,
        long id,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.SoftDeleteAsync(id, dataSource, cancellationToken);
    }

    public async Task<bool> SetClusterActiveAsync(
        [Service] MicroservicesClusterService service,
        long id,
        bool active,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.SetActiveAsync(id, active, dataSource, cancellationToken);
    }

    // ========================================
    // USERS - MUTATIONS
    // ========================================

    public async Task<UserDto> CreateUserAsync(
        [Service] UserService service,
        string? name,
        string? email,
        string? peoplesoft = null,
        bool active = true,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(peoplesoft, name, email, active, dataSource, cancellationToken);
    }

    public async Task<UserDto?> UpdateUserAsync(
        [Service] UserService service,
        Guid id,
        string? name = null,
        string? email = null,
        string? peoplesoft = null,
        bool? active = null,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, peoplesoft, name, email, active, dataSource, cancellationToken);
    }

    public async Task<bool> DeleteUserAsync(
        [Service] UserService service,
        Guid id,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(id, dataSource, cancellationToken);
    }

    public async Task<bool> SetUserActiveAsync(
        [Service] UserService service,
        Guid id,
        bool active,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.SetActiveAsync(id, active, dataSource, cancellationToken);
    }

    // ========================================
    // ACTIVITY LOGS - MUTATIONS
    // ========================================

    public async Task<ActivityLogDto> CreateActivityLogAsync(
        [Service] ActivityLogService service,
        long? eventTypeId,
        string? entityName,
        Guid? entityId,
        string? description,
        Guid? userId,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(eventTypeId, entityName, entityId, description, userId, dataSource, cancellationToken);
    }

    public async Task<bool> DeleteActivityLogAsync(
        [Service] ActivityLogService service,
        Guid id,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(id, dataSource, cancellationToken);
    }

    // ========================================
    // EVENT TYPES - MUTATIONS
    // ========================================

    public async Task<EventTypeDto> CreateEventTypeAsync(
        [Service] EventTypeService service,
        string description,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(description, dataSource, cancellationToken);
    }

    public async Task<EventTypeDto?> UpdateEventTypeAsync(
        [Service] EventTypeService service,
        long id,
        string description,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, description, dataSource, cancellationToken);
    }

    public async Task<bool> DeleteEventTypeAsync(
        [Service] EventTypeService service,
        long id,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(id, dataSource, cancellationToken);
    }

    // ========================================
    // CREDENTIALS - MUTATIONS
    // ========================================

    public async Task<CoreConnectorCredentialDto> CreateCredentialAsync(
        [Service] CoreConnectorCredentialService service,
        string? user,
        string? password,
        string? key = null,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(user, password, key, dataSource, cancellationToken);
    }

    public async Task<CoreConnectorCredentialDto?> UpdateCredentialAsync(
        [Service] CoreConnectorCredentialService service,
        long id,
        string? user = null,
        string? password = null,
        string? key = null,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, user, password, key, dataSource, cancellationToken);
    }

    public async Task<bool> DeleteCredentialAsync(
        [Service] CoreConnectorCredentialService service,
        long id,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(id, dataSource, cancellationToken);
    }

    // ========================================
    // CONNECTORS - MUTATIONS
    // ========================================

    public async Task<MicroserviceCoreConnectorDto> CreateConnectorAsync(
        [Service] MicroserviceCoreConnectorService service,
        long? credentialId,
        long? microserviceId,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(credentialId, microserviceId, dataSource, cancellationToken);
    }

    public async Task<MicroserviceCoreConnectorDto?> UpdateConnectorAsync(
        [Service] MicroserviceCoreConnectorService service,
        long id,
        long? credentialId = null,
        long? microserviceId = null,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, credentialId, microserviceId, dataSource, cancellationToken);
    }

    public async Task<bool> DeleteConnectorAsync(
        [Service] MicroserviceCoreConnectorService service,
        long id,
        DataSourceType dataSource = DataSourceType.SqlServer,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(id, dataSource, cancellationToken);
    }
}
