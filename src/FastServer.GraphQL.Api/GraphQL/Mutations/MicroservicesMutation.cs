using FastServer.Application.DTOs.Microservices;
using FastServer.Application.Services.Microservices;
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
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(clusterId, name, active, coreConnection, cancellationToken);
    }

    public async Task<MicroserviceRegisterDto?> UpdateMicroserviceAsync(
        [Service] MicroserviceRegisterService service,
        long id,
        long? clusterId = null,
        string? name = null,
        bool? active = null,
        bool? coreConnection = null,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, clusterId, name, active, coreConnection, cancellationToken);
    }

    public async Task<bool> SoftDeleteMicroserviceAsync(
        [Service] MicroserviceRegisterService service,
        long id,
        CancellationToken cancellationToken = default)
    {
        return await service.SoftDeleteAsync(id, cancellationToken);
    }

    public async Task<bool> SetMicroserviceActiveAsync(
        [Service] MicroserviceRegisterService service,
        long id,
        bool active,
        CancellationToken cancellationToken = default)
    {
        return await service.SetActiveAsync(id, active, cancellationToken);
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
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(name, serverName, serverIp, active, cancellationToken);
    }

    public async Task<MicroservicesClusterDto?> UpdateClusterAsync(
        [Service] MicroservicesClusterService service,
        long id,
        string? name = null,
        string? serverName = null,
        string? serverIp = null,
        bool? active = null,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, name, serverName, serverIp, active, cancellationToken);
    }

    public async Task<bool> SoftDeleteClusterAsync(
        [Service] MicroservicesClusterService service,
        long id,
        CancellationToken cancellationToken = default)
    {
        return await service.SoftDeleteAsync(id, cancellationToken);
    }

    public async Task<bool> SetClusterActiveAsync(
        [Service] MicroservicesClusterService service,
        long id,
        bool active,
        CancellationToken cancellationToken = default)
    {
        return await service.SetActiveAsync(id, active, cancellationToken);
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
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(peoplesoft, name, email, active, cancellationToken);
    }

    public async Task<UserDto?> UpdateUserAsync(
        [Service] UserService service,
        Guid id,
        string? name = null,
        string? email = null,
        string? peoplesoft = null,
        bool? active = null,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, peoplesoft, name, email, active, cancellationToken);
    }

    public async Task<bool> DeleteUserAsync(
        [Service] UserService service,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(id, cancellationToken);
    }

    public async Task<bool> SetUserActiveAsync(
        [Service] UserService service,
        Guid id,
        bool active,
        CancellationToken cancellationToken = default)
    {
        return await service.SetActiveAsync(id, active, cancellationToken);
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
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(eventTypeId, entityName, entityId, description, userId, cancellationToken);
    }

    public async Task<bool> DeleteActivityLogAsync(
        [Service] ActivityLogService service,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(id, cancellationToken);
    }

    // ========================================
    // EVENT TYPES - MUTATIONS
    // ========================================

    public async Task<EventTypeDto> CreateEventTypeAsync(
        [Service] EventTypeService service,
        string description,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(description, cancellationToken);
    }

    public async Task<EventTypeDto?> UpdateEventTypeAsync(
        [Service] EventTypeService service,
        long id,
        string description,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, description, cancellationToken);
    }

    public async Task<bool> DeleteEventTypeAsync(
        [Service] EventTypeService service,
        long id,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(id, cancellationToken);
    }

    // ========================================
    // CREDENTIALS - MUTATIONS
    // ========================================

    public async Task<CoreConnectorCredentialDto> CreateCredentialAsync(
        [Service] CoreConnectorCredentialService service,
        string? user,
        string? password,
        string? key = null,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(user, password, key, cancellationToken);
    }

    public async Task<CoreConnectorCredentialDto?> UpdateCredentialAsync(
        [Service] CoreConnectorCredentialService service,
        long id,
        string? user = null,
        string? password = null,
        string? key = null,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, user, password, key, cancellationToken);
    }

    public async Task<bool> DeleteCredentialAsync(
        [Service] CoreConnectorCredentialService service,
        long id,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(id, cancellationToken);
    }

    // ========================================
    // CONNECTORS - MUTATIONS
    // ========================================

    public async Task<MicroserviceCoreConnectorDto> CreateConnectorAsync(
        [Service] MicroserviceCoreConnectorService service,
        long? credentialId,
        long? microserviceId,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(credentialId, microserviceId, cancellationToken);
    }

    public async Task<MicroserviceCoreConnectorDto?> UpdateConnectorAsync(
        [Service] MicroserviceCoreConnectorService service,
        long id,
        long? credentialId = null,
        long? microserviceId = null,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, credentialId, microserviceId, cancellationToken);
    }

    public async Task<bool> DeleteConnectorAsync(
        [Service] MicroserviceCoreConnectorService service,
        long id,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(id, cancellationToken);
    }
}
