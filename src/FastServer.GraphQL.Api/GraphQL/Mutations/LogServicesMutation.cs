using FastServer.Application.DTOs;
using FastServer.Application.Interfaces;
using FastServer.GraphQL.Api.GraphQL.Types;

namespace FastServer.GraphQL.Api.GraphQL.Mutations;

/// <summary>
/// Mutaciones GraphQL para LogServices
/// </summary>
[ExtendObjectType("Mutation")]
public class LogServicesMutation
{
    /// <summary>
    /// Crea un nuevo log de servicios
    /// </summary>
    [GraphQLDescription("Crea un nuevo log de servicios")]
    public async Task<LogServicesHeaderDto> CreateLogServicesHeader(
        [Service] ILogServicesHeaderService service,
        [GraphQLDescription("Datos del log a crear")] CreateLogServicesHeaderInput input,
        CancellationToken cancellationToken = default)
    {
        var dto = new CreateLogServicesHeaderDto
        {
            LogDateIn = input.LogDateIn,
            LogDateOut = input.LogDateOut,
            LogState = input.LogState,
            LogMethodUrl = input.LogMethodUrl,
            LogMethodName = input.LogMethodName,
            LogFsId = input.LogFsId,
            MethodDescription = input.MethodDescription,
            TciIpPort = input.TciIpPort,
            ErrorCode = input.ErrorCode,
            ErrorDescription = input.ErrorDescription,
            IpFs = input.IpFs,
            TypeProcess = input.TypeProcess,
            LogNodo = input.LogNodo,
            HttpMethod = input.HttpMethod,
            MicroserviceName = input.MicroserviceName,
            RequestDuration = input.RequestDuration,
            TransactionId = input.TransactionId,
            UserId = input.UserId,
            SessionId = input.SessionId,
            RequestId = input.RequestId
        };

        return await service.CreateAsync(dto, input.DataSource, cancellationToken);
    }

    /// <summary>
    /// Actualiza un log de servicios
    /// </summary>
    [GraphQLDescription("Actualiza un log de servicios existente")]
    public async Task<LogServicesHeaderDto> UpdateLogServicesHeader(
        [Service] ILogServicesHeaderService service,
        [GraphQLDescription("Datos a actualizar")] UpdateLogServicesHeaderInput input,
        CancellationToken cancellationToken = default)
    {
        var dto = new UpdateLogServicesHeaderDto
        {
            LogId = input.LogId,
            LogDateOut = input.LogDateOut,
            LogState = input.LogState,
            ErrorCode = input.ErrorCode,
            ErrorDescription = input.ErrorDescription,
            RequestDuration = input.RequestDuration
        };

        return await service.UpdateAsync(dto, input.DataSource, cancellationToken);
    }

    /// <summary>
    /// Elimina un log de servicios
    /// </summary>
    [GraphQLDescription("Elimina un log de servicios por su ID")]
    public async Task<bool> DeleteLogServicesHeader(
        [Service] ILogServicesHeaderService service,
        [GraphQLDescription("ID del log a eliminar")] long logId,
        [GraphQLDescription("Origen de datos")] FastServer.Domain.Enums.DataSourceType? dataSource = null,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(logId, dataSource, cancellationToken);
    }
}

/// <summary>
/// Mutaciones GraphQL para LogMicroservice
/// </summary>
[ExtendObjectType("Mutation")]
public class LogMicroserviceMutation
{
    /// <summary>
    /// Crea un nuevo log de microservicio
    /// </summary>
    [GraphQLDescription("Crea un nuevo log de microservicio")]
    public async Task<LogMicroserviceDto> CreateLogMicroservice(
        [Service] ILogMicroserviceService service,
        [GraphQLDescription("Datos del log a crear")] CreateLogMicroserviceInput input,
        CancellationToken cancellationToken = default)
    {
        var dto = new CreateLogMicroserviceDto
        {
            LogId = input.LogId,
            LogDate = input.LogDate,
            LogLevel = input.LogLevel,
            LogMicroserviceText = input.LogMicroserviceText
        };

        return await service.CreateAsync(dto, input.DataSource, cancellationToken);
    }

    /// <summary>
    /// Elimina un log de microservicio
    /// </summary>
    [GraphQLDescription("Elimina un log de microservicio por su ID")]
    public async Task<bool> DeleteLogMicroservice(
        [Service] ILogMicroserviceService service,
        [GraphQLDescription("ID del log a eliminar")] long logId,
        [GraphQLDescription("Origen de datos")] FastServer.Domain.Enums.DataSourceType? dataSource = null,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(logId, dataSource, cancellationToken);
    }
}

/// <summary>
/// Mutaciones GraphQL para LogServicesContent
/// </summary>
[ExtendObjectType("Mutation")]
public class LogServicesContentMutation
{
    /// <summary>
    /// Crea un nuevo contenido de log
    /// </summary>
    [GraphQLDescription("Crea un nuevo contenido de log de servicios")]
    public async Task<LogServicesContentDto> CreateLogServicesContent(
        [Service] ILogServicesContentService service,
        [GraphQLDescription("Datos del contenido a crear")] CreateLogServicesContentInput input,
        CancellationToken cancellationToken = default)
    {
        var dto = new CreateLogServicesContentDto
        {
            LogId = input.LogId,
            LogServicesDate = input.LogServicesDate,
            LogServicesLogLevel = input.LogServicesLogLevel,
            LogServicesState = input.LogServicesState,
            LogServicesContentText = input.LogServicesContentText
        };

        return await service.CreateAsync(dto, input.DataSource, cancellationToken);
    }

    /// <summary>
    /// Elimina un contenido de log
    /// </summary>
    [GraphQLDescription("Elimina un contenido de log por su ID")]
    public async Task<bool> DeleteLogServicesContent(
        [Service] ILogServicesContentService service,
        [GraphQLDescription("ID del log a eliminar")] long logId,
        [GraphQLDescription("Origen de datos")] FastServer.Domain.Enums.DataSourceType? dataSource = null,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(logId, dataSource, cancellationToken);
    }
}
