using FastServer.Application.DTOs.Microservices;
using FastServer.Application.Interfaces.Microservices;
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

    [GraphQLDescription("Crea un nuevo registro de microservicio en el sistema")]
    public async Task<MicroserviceRegisterDto> CreateMicroserviceAsync(
        [Service] IMicroserviceRegisterService service,
        [GraphQLDescription("Nombre del microservicio")] string name,
        [GraphQLDescription("Indica si el microservicio está activo (por defecto: true)")] bool active = true,
        [GraphQLDescription("Indica si tiene conexión al Core (por defecto: false)")] bool coreConnection = false,
        [GraphQLDescription("URL base SOAP del microservicio (opcional)")] string? soapBase = null,
        [GraphQLDescription("ID del tipo de microservicio (opcional)")] Guid? microserviceTypeId = null,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(name, active, coreConnection, soapBase, microserviceTypeId, cancellationToken);
    }

    [GraphQLDescription("Actualiza un registro de microservicio existente. Solo se modifican los campos proporcionados")]
    public async Task<MicroserviceRegisterDto?> UpdateMicroserviceAsync(
        [Service] IMicroserviceRegisterService service,
        [GraphQLDescription("ID del microservicio a actualizar")] Guid id,
        [GraphQLDescription("Nuevo nombre del microservicio (opcional)")] string? name = null,
        [GraphQLDescription("Nuevo estado activo/inactivo (opcional)")] bool? active = null,
        [GraphQLDescription("Nueva conexión al Core (opcional)")] bool? coreConnection = null,
        [GraphQLDescription("Nueva URL base SOAP (opcional)")] string? soapBase = null,
        [GraphQLDescription("Nuevo ID del tipo de microservicio (opcional)")] Guid? microserviceTypeId = null,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, name, active, coreConnection, soapBase, microserviceTypeId, cancellationToken);
    }

    [GraphQLDescription("Elimina lógicamente un microservicio (soft delete). Retorna true si se eliminó correctamente")]
    public async Task<bool> SoftDeleteMicroserviceAsync(
        [Service] IMicroserviceRegisterService service,
        [GraphQLDescription("ID del microservicio a eliminar")] Guid id,
        CancellationToken cancellationToken = default)
    {
        return await service.SoftDeleteAsync(id, cancellationToken);
    }

    [GraphQLDescription("Activa o desactiva un microservicio. Retorna true si se actualizó correctamente")]
    public async Task<bool> SetMicroserviceActiveAsync(
        [Service] IMicroserviceRegisterService service,
        [GraphQLDescription("ID del microservicio")] Guid id,
        [GraphQLDescription("true para activar, false para desactivar")] bool active,
        CancellationToken cancellationToken = default)
    {
        return await service.SetActiveAsync(id, active, cancellationToken);
    }

    // ========================================
    // CLUSTERS - MUTATIONS
    // ========================================

    [GraphQLDescription("Crea un nuevo cluster de microservicios")]
    public async Task<MicroservicesClusterDto> CreateClusterAsync(
        [Service] MicroservicesClusterService service,
        [GraphQLDescription("Nombre del cluster")] string name,
        [GraphQLDescription("Nombre del servidor (opcional)")] string? serverName = null,
        [GraphQLDescription("Dirección IP del servidor (opcional)")] string? serverIp = null,
        [GraphQLDescription("Protocolo del cluster (opcional)")] string? protocol = null,
        [GraphQLDescription("Indica si el cluster está activo (por defecto: true)")] bool active = true,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(name, serverName, serverIp, protocol, active, cancellationToken);
    }

    [GraphQLDescription("Actualiza un cluster existente. Solo se modifican los campos proporcionados")]
    public async Task<MicroservicesClusterDto?> UpdateClusterAsync(
        [Service] MicroservicesClusterService service,
        [GraphQLDescription("ID del cluster a actualizar")] Guid id,
        [GraphQLDescription("Nuevo nombre del cluster (opcional)")] string? name = null,
        [GraphQLDescription("Nuevo nombre del servidor (opcional)")] string? serverName = null,
        [GraphQLDescription("Nueva IP del servidor (opcional)")] string? serverIp = null,
        [GraphQLDescription("Nuevo protocolo del cluster (opcional)")] string? protocol = null,
        [GraphQLDescription("Nuevo estado activo/inactivo (opcional)")] bool? active = null,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, name, serverName, serverIp, protocol, active, cancellationToken);
    }

    [GraphQLDescription("Elimina lógicamente un cluster (soft delete). Retorna true si se eliminó correctamente")]
    public async Task<bool> SoftDeleteClusterAsync(
        [Service] MicroservicesClusterService service,
        [GraphQLDescription("ID del cluster a eliminar")] Guid id,
        CancellationToken cancellationToken = default)
    {
        return await service.SoftDeleteAsync(id, cancellationToken);
    }

    [GraphQLDescription("Activa o desactiva un cluster. Retorna true si se actualizó correctamente")]
    public async Task<bool> SetClusterActiveAsync(
        [Service] MicroservicesClusterService service,
        [GraphQLDescription("ID del cluster")] Guid id,
        [GraphQLDescription("true para activar, false para desactivar")] bool active,
        CancellationToken cancellationToken = default)
    {
        return await service.SetActiveAsync(id, active, cancellationToken);
    }

    // ========================================
    // USERS - MUTATIONS
    // ========================================

    [GraphQLDescription("Crea un nuevo usuario en el sistema")]
    public async Task<UserDto> CreateUserAsync(
        [Service] UserService service,
        [GraphQLDescription("Nombre del usuario")] string name,
        [GraphQLDescription("Correo electrónico del usuario")] string email,
        [GraphQLDescription("Código PeopleSoft del usuario (opcional)")] string? peoplesoft = null,
        [GraphQLDescription("Indica si el usuario está activo (por defecto: true)")] bool active = true,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(peoplesoft, name, email, active, cancellationToken);
    }

    [GraphQLDescription("Actualiza un usuario existente. Solo se modifican los campos proporcionados")]
    public async Task<UserDto?> UpdateUserAsync(
        [Service] UserService service,
        [GraphQLDescription("ID (GUID) del usuario a actualizar")] Guid id,
        [GraphQLDescription("Nuevo nombre del usuario (opcional)")] string? name = null,
        [GraphQLDescription("Nuevo correo electrónico (opcional)")] string? email = null,
        [GraphQLDescription("Nuevo código PeopleSoft (opcional)")] string? peoplesoft = null,
        [GraphQLDescription("Nuevo estado activo/inactivo (opcional)")] bool? active = null,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, peoplesoft, name, email, active, cancellationToken);
    }

    [GraphQLDescription("Elimina permanentemente un usuario. Retorna true si se eliminó correctamente")]
    public async Task<bool> DeleteUserAsync(
        [Service] UserService service,
        [GraphQLDescription("ID (GUID) del usuario a eliminar")] Guid id,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(id, cancellationToken);
    }

    [GraphQLDescription("Activa o desactiva un usuario. Retorna true si se actualizó correctamente")]
    public async Task<bool> SetUserActiveAsync(
        [Service] UserService service,
        [GraphQLDescription("ID (GUID) del usuario")] Guid id,
        [GraphQLDescription("true para activar, false para desactivar")] bool active,
        CancellationToken cancellationToken = default)
    {
        return await service.SetActiveAsync(id, active, cancellationToken);
    }

    // ========================================
    // ACTIVITY LOGS - MUTATIONS
    // ========================================

    [GraphQLDescription("Registra un nuevo log de actividad en el sistema")]
    public async Task<ActivityLogDto> CreateActivityLogAsync(
        [Service] ActivityLogService service,
        [GraphQLDescription("ID del tipo de evento asociado")] Guid? eventTypeId,
        [GraphQLDescription("Nombre de la entidad afectada")] string? entityName,
        [GraphQLDescription("ID (GUID) de la entidad afectada")] Guid? entityId,
        [GraphQLDescription("Descripción de la actividad realizada")] string? description,
        [GraphQLDescription("ID (GUID) del usuario que realizó la acción")] Guid? userId,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(eventTypeId, entityName, entityId, description, userId, cancellationToken);
    }

    [GraphQLDescription("Elimina permanentemente un log de actividad. Retorna true si se eliminó correctamente")]
    public async Task<bool> DeleteActivityLogAsync(
        [Service] ActivityLogService service,
        [GraphQLDescription("ID (GUID) del log de actividad a eliminar")] Guid id,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(id, cancellationToken);
    }

    // ========================================
    // EVENT TYPES - MUTATIONS
    // ========================================

    [GraphQLDescription("Crea un nuevo tipo de evento para clasificar logs de actividad")]
    public async Task<EventTypeDto> CreateEventTypeAsync(
        [Service] EventTypeService service,
        [GraphQLDescription("Descripción del tipo de evento (ej: 'Creación', 'Eliminación')")] string description,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(description, cancellationToken);
    }

    [GraphQLDescription("Actualiza la descripción de un tipo de evento existente")]
    public async Task<EventTypeDto?> UpdateEventTypeAsync(
        [Service] EventTypeService service,
        [GraphQLDescription("ID del tipo de evento a actualizar")] Guid id,
        [GraphQLDescription("Nueva descripción del tipo de evento")] string description,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, description, cancellationToken);
    }

    [GraphQLDescription("Elimina permanentemente un tipo de evento. Retorna true si se eliminó correctamente")]
    public async Task<bool> DeleteEventTypeAsync(
        [Service] EventTypeService service,
        [GraphQLDescription("ID del tipo de evento a eliminar")] Guid id,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(id, cancellationToken);
    }

    // ========================================
    // CREDENTIALS - MUTATIONS
    // ========================================

    [GraphQLDescription("Crea una nueva credencial para conexión al Core Connector")]
    public async Task<CoreConnectorCredentialDto> CreateCredentialAsync(
        [Service] CoreConnectorCredentialService service,
        [GraphQLDescription("Nombre de usuario de la credencial")] string? user,
        [GraphQLDescription("Contraseña de la credencial")] string? password,
        [GraphQLDescription("Clave de autenticación adicional (opcional)")] string? key = null,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(user, password, key, cancellationToken);
    }

    [GraphQLDescription("Actualiza una credencial existente. Solo se modifican los campos proporcionados")]
    public async Task<CoreConnectorCredentialDto?> UpdateCredentialAsync(
        [Service] CoreConnectorCredentialService service,
        [GraphQLDescription("ID de la credencial a actualizar")] Guid id,
        [GraphQLDescription("Nuevo nombre de usuario (opcional)")] string? user = null,
        [GraphQLDescription("Nueva contraseña (opcional)")] string? password = null,
        [GraphQLDescription("Nueva clave de autenticación (opcional)")] string? key = null,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, user, password, key, cancellationToken);
    }

    [GraphQLDescription("Elimina permanentemente una credencial. Retorna true si se eliminó correctamente")]
    public async Task<bool> DeleteCredentialAsync(
        [Service] CoreConnectorCredentialService service,
        [GraphQLDescription("ID de la credencial a eliminar")] Guid id,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(id, cancellationToken);
    }

    // ========================================
    // CONNECTORS - MUTATIONS
    // ========================================

    [GraphQLDescription("Crea una nueva conexión entre un microservicio y el Core Connector")]
    public async Task<MicroserviceCoreConnectorDto> CreateConnectorAsync(
        [Service] MicroserviceCoreConnectorService service,
        [GraphQLDescription("ID de la credencial a asociar")] Guid? credentialId,
        [GraphQLDescription("ID del microservicio a conectar")] Guid? microserviceId,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(credentialId, microserviceId, cancellationToken);
    }

    [GraphQLDescription("Actualiza una conexión existente entre microservicio y Core Connector")]
    public async Task<MicroserviceCoreConnectorDto?> UpdateConnectorAsync(
        [Service] MicroserviceCoreConnectorService service,
        [GraphQLDescription("ID del conector a actualizar")] Guid id,
        [GraphQLDescription("Nuevo ID de credencial (opcional)")] Guid? credentialId = null,
        [GraphQLDescription("Nuevo ID de microservicio (opcional)")] Guid? microserviceId = null,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, credentialId, microserviceId, cancellationToken);
    }

    [GraphQLDescription("Elimina permanentemente una conexión al Core Connector. Retorna true si se eliminó correctamente")]
    public async Task<bool> DeleteConnectorAsync(
        [Service] MicroserviceCoreConnectorService service,
        [GraphQLDescription("ID del conector a eliminar")] Guid id,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(id, cancellationToken);
    }

    // ========================================
    // MICROSERVICE METHODS - MUTATIONS
    // ========================================

    [GraphQLDescription("Crea un nuevo método de microservicio")]
    public async Task<MicroserviceMethodDto> CreateMethodAsync(
        [Service] MicroserviceMethodService service,
        [GraphQLDescription("ID del microservicio al que pertenece")] Guid microserviceId,
        [GraphQLDescription("Nombre del método/endpoint")] string? name = null,
        [GraphQLDescription("URL del método/endpoint")] string? url = null,
        [GraphQLDescription("Método HTTP (GET, POST, etc.)")] string? httpMethod = null,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(microserviceId, name, url, httpMethod, cancellationToken);
    }

    [GraphQLDescription("Actualiza un método de microservicio existente. Solo se modifican los campos proporcionados")]
    public async Task<MicroserviceMethodDto?> UpdateMethodAsync(
        [Service] MicroserviceMethodService service,
        [GraphQLDescription("ID del método a actualizar")] Guid id,
        [GraphQLDescription("Nuevo ID de microservicio (opcional)")] Guid? microserviceId = null,
        [GraphQLDescription("Nuevo nombre del método (opcional)")] string? name = null,
        [GraphQLDescription("Nueva URL del método (opcional)")] string? url = null,
        [GraphQLDescription("Nuevo método HTTP (opcional)")] string? httpMethod = null,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, microserviceId, name, url, httpMethod, cancellationToken);
    }

    [GraphQLDescription("Elimina lógicamente un método de microservicio (soft delete). Retorna true si se eliminó correctamente")]
    public async Task<bool> SoftDeleteMethodAsync(
        [Service] MicroserviceMethodService service,
        [GraphQLDescription("ID del método a eliminar")] Guid id,
        CancellationToken cancellationToken = default)
    {
        return await service.SoftDeleteAsync(id, cancellationToken);
    }

    // ========================================
    // MICROSERVICES REGISTER TYPES - MUTATIONS
    // ========================================

    [GraphQLDescription("Crea un nuevo tipo de registro de microservicio")]
    public async Task<MicroservicesRegisterTypeDto> CreateMicroservicesRegisterTypeAsync(
        [Service] IMicroservicesRegisterTypeService service,
        [GraphQLDescription("Nombre del tipo de registro (opcional)")] string? name = null,
        [GraphQLDescription("Descripción del tipo de registro (opcional)")] string? description = null,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(name, description, cancellationToken);
    }

    [GraphQLDescription("Actualiza un tipo de registro de microservicio existente. Solo se modifican los campos proporcionados")]
    public async Task<MicroservicesRegisterTypeDto?> UpdateMicroservicesRegisterTypeAsync(
        [Service] IMicroservicesRegisterTypeService service,
        [GraphQLDescription("ID del tipo de registro a actualizar")] Guid id,
        [GraphQLDescription("Nuevo nombre del tipo de registro (opcional)")] string? name = null,
        [GraphQLDescription("Nueva descripción del tipo de registro (opcional)")] string? description = null,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, name, description, cancellationToken);
    }

    [GraphQLDescription("Elimina permanentemente un tipo de registro de microservicio. Retorna true si se eliminó correctamente")]
    public async Task<bool> DeleteMicroservicesRegisterTypeAsync(
        [Service] IMicroservicesRegisterTypeService service,
        [GraphQLDescription("ID del tipo de registro a eliminar")] Guid id,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(id, cancellationToken);
    }

    // ========================================
    // NODOS - MUTATIONS
    // ========================================

    [GraphQLDescription("Crea un nuevo nodo (relación método-cluster)")]
    public async Task<NodoDto> CreateNodoAsync(
        [Service] INodoService service,
        [GraphQLDescription("ID del método de microservicio")] Guid methodId,
        [GraphQLDescription("ID del cluster de microservicios")] Guid clusterId,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(methodId, clusterId, cancellationToken);
    }

    [GraphQLDescription("Actualiza un nodo existente. Solo se modifican los campos proporcionados")]
    public async Task<NodoDto?> UpdateNodoAsync(
        [Service] INodoService service,
        [GraphQLDescription("ID del nodo a actualizar")] Guid id,
        [GraphQLDescription("Nuevo ID del método de microservicio (opcional)")] Guid? methodId = null,
        [GraphQLDescription("Nuevo ID del cluster de microservicios (opcional)")] Guid? clusterId = null,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, methodId, clusterId, cancellationToken);
    }

    [GraphQLDescription("Elimina permanentemente un nodo. Retorna true si se eliminó correctamente")]
    public async Task<bool> DeleteNodoAsync(
        [Service] INodoService service,
        [GraphQLDescription("ID del nodo a eliminar")] Guid id,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(id, cancellationToken);
    }

    // ========================================
    // FASTSERVER CLUSTERS - MUTATIONS
    // ========================================

    [GraphQLDescription("Crea un nuevo cluster de FastServer con GUID v7")]
    public async Task<FastServerClusterDto> CreateFastServerClusterAsync(
        [Service] FastServerClusterService service,
        [GraphQLDescription("Nombre del cluster")] string name,
        [GraphQLDescription("URL base del cluster (opcional)")] string? url = null,
        [GraphQLDescription("Versión del software desplegada (opcional)")] string? version = null,
        [GraphQLDescription("Nombre del servidor (opcional)")] string? serverName = null,
        [GraphQLDescription("Dirección IP del servidor (opcional)")] string? serverIp = null,
        [GraphQLDescription("Indica si el cluster está activo (por defecto: true)")] bool active = true,
        CancellationToken cancellationToken = default)
    {
        return await service.CreateAsync(name, url, version, serverName, serverIp, active, cancellationToken);
    }

    [GraphQLDescription("Actualiza un cluster de FastServer existente. Solo se modifican los campos proporcionados")]
    public async Task<FastServerClusterDto?> UpdateFastServerClusterAsync(
        [Service] FastServerClusterService service,
        [GraphQLDescription("ID (GUID) del cluster a actualizar")] Guid id,
        [GraphQLDescription("Nuevo nombre del cluster (opcional)")] string? name = null,
        [GraphQLDescription("Nueva URL base del cluster (opcional)")] string? url = null,
        [GraphQLDescription("Nueva versión del software (opcional)")] string? version = null,
        [GraphQLDescription("Nuevo nombre del servidor (opcional)")] string? serverName = null,
        [GraphQLDescription("Nueva IP del servidor (opcional)")] string? serverIp = null,
        [GraphQLDescription("Nuevo estado activo/inactivo (opcional)")] bool? active = null,
        CancellationToken cancellationToken = default)
    {
        return await service.UpdateAsync(id, name, url, version, serverName, serverIp, active, cancellationToken);
    }

    [GraphQLDescription("Elimina lógicamente un cluster de FastServer (soft delete). Retorna true si se eliminó correctamente")]
    public async Task<bool> SoftDeleteFastServerClusterAsync(
        [Service] FastServerClusterService service,
        [GraphQLDescription("ID (GUID) del cluster a eliminar")] Guid id,
        CancellationToken cancellationToken = default)
    {
        return await service.SoftDeleteAsync(id, cancellationToken);
    }

    [GraphQLDescription("Activa o desactiva un cluster de FastServer. Retorna true si se actualizó correctamente")]
    public async Task<bool> SetFastServerClusterActiveAsync(
        [Service] FastServerClusterService service,
        [GraphQLDescription("ID (GUID) del cluster")] Guid id,
        [GraphQLDescription("true para activar, false para desactivar")] bool active,
        CancellationToken cancellationToken = default)
    {
        return await service.SetActiveAsync(id, active, cancellationToken);
    }
}
