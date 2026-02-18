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
    [GraphQLDescription("Crea un nuevo log de servicios en FastServer_Logs (PostgreSQL)")]
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

        return await service.CreateAsync(dto, cancellationToken);
    }

    /// <summary>
    /// Crea múltiples logs de servicios en una sola operación
    /// </summary>
    [GraphQLDescription("Crea múltiples logs de servicios en una sola operación atómica en FastServer_Logs (PostgreSQL)")]
    public async Task<BulkInsertResultDto<LogServicesHeaderDto>> BulkCreateLogServicesHeader(
        [Service] ILogServicesHeaderService service,
        [GraphQLDescription("Datos de los logs a crear")] BulkCreateLogServicesHeaderInput input,
        CancellationToken cancellationToken = default)
    {
        var dtos = input.Items.Select(i => new CreateLogServicesHeaderDto
        {
            LogDateIn = i.LogDateIn,
            LogDateOut = i.LogDateOut,
            LogState = i.LogState,
            LogMethodUrl = i.LogMethodUrl,
            LogMethodName = i.LogMethodName,
            LogFsId = i.LogFsId,
            MethodDescription = i.MethodDescription,
            TciIpPort = i.TciIpPort,
            ErrorCode = i.ErrorCode,
            ErrorDescription = i.ErrorDescription,
            IpFs = i.IpFs,
            TypeProcess = i.TypeProcess,
            LogNodo = i.LogNodo,
            HttpMethod = i.HttpMethod,
            MicroserviceName = i.MicroserviceName,
            RequestDuration = i.RequestDuration,
            TransactionId = i.TransactionId,
            UserId = i.UserId,
            SessionId = i.SessionId,
            RequestId = i.RequestId
        });

        return await service.CreateBulkAsync(dtos, cancellationToken);
    }

    /// <summary>
    /// Actualiza un log de servicios
    /// </summary>
    [GraphQLDescription("Actualiza un log de servicios existente en FastServer_Logs (PostgreSQL)")]
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
            LogMethodName = input.LogMethodName,
            MethodDescription = input.MethodDescription,
            TciIpPort = input.TciIpPort,
            IpFs = input.IpFs,
            TypeProcess = input.TypeProcess,
            LogNodo = input.LogNodo,
            MicroserviceName = input.MicroserviceName,
            UserId = input.UserId,
            ErrorCode = input.ErrorCode,
            ErrorDescription = input.ErrorDescription,
            RequestDuration = input.RequestDuration
        };

        return await service.UpdateAsync(dto, cancellationToken);
    }

    /// <summary>
    /// Actualiza múltiples logs de servicios en una sola operación
    /// </summary>
    [GraphQLDescription("Actualiza múltiples logs de servicios en una sola operación atómica en FastServer_Logs (PostgreSQL)")]
    public async Task<BulkUpdateResultDto<LogServicesHeaderDto>> BulkUpdateLogServicesHeader(
        [Service] ILogServicesHeaderService service,
        [GraphQLDescription("Datos de los logs a actualizar")] BulkUpdateLogServicesHeaderInput input,
        CancellationToken cancellationToken = default)
    {
        var dtos = input.Items.Select(i => new UpdateLogServicesHeaderDto
        {
            LogId = i.LogId,
            LogDateOut = i.LogDateOut,
            LogState = i.LogState,
            LogMethodName = i.LogMethodName,
            MethodDescription = i.MethodDescription,
            TciIpPort = i.TciIpPort,
            IpFs = i.IpFs,
            TypeProcess = i.TypeProcess,
            LogNodo = i.LogNodo,
            MicroserviceName = i.MicroserviceName,
            UserId = i.UserId,
            ErrorCode = i.ErrorCode,
            ErrorDescription = i.ErrorDescription,
            RequestDuration = i.RequestDuration
        });

        return await service.UpdateBulkAsync(dtos, cancellationToken);
    }

    /// <summary>
    /// Elimina un log de servicios
    /// </summary>
    [GraphQLDescription("Elimina un log de servicios por su ID desde FastServer_Logs (PostgreSQL)")]
    public async Task<bool> DeleteLogServicesHeader(
        [Service] ILogServicesHeaderService service,
        [GraphQLDescription("ID del log a eliminar")] long logId,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(logId, cancellationToken);
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
    [GraphQLDescription("Crea un nuevo log de microservicio en FastServer_Logs (PostgreSQL)")]
    public async Task<LogMicroserviceDto> CreateLogMicroservice(
        [Service] ILogMicroserviceService service,
        [GraphQLDescription("Datos del log a crear")] CreateLogMicroserviceInput input,
        CancellationToken cancellationToken = default)
    {
        var dto = new CreateLogMicroserviceDto
        {
            LogId = input.LogId,
            RequestId = input.RequestId,
            EventName = input.EventName,
            LogDate = input.LogDate,
            LogLevel = input.LogLevel,
            LogMicroserviceText = input.LogMicroserviceText
        };

        return await service.CreateAsync(dto, cancellationToken);
    }

    /// <summary>
    /// Crea múltiples logs de microservicio en una sola operación
    /// </summary>
    [GraphQLDescription("Crea múltiples logs de microservicio en una sola operación en FastServer_Logs (PostgreSQL)")]
    public async Task<BulkInsertResultDto<LogMicroserviceDto>> BulkCreateLogMicroservice(
        [Service] ILogMicroserviceService service,
        [GraphQLDescription("Datos de los logs a crear")] BulkCreateLogMicroserviceInput input,
        CancellationToken cancellationToken = default)
    {
        var dtos = input.Items.Select(i => new CreateLogMicroserviceDto
        {
            LogId = i.LogId,
            RequestId = i.RequestId,
            EventName = i.EventName,
            LogDate = i.LogDate,
            LogLevel = i.LogLevel,
            LogMicroserviceText = i.LogMicroserviceText
        });

        return await service.CreateBulkAsync(dtos, cancellationToken);
    }

    /// <summary>
    /// Actualiza un log de microservicio
    /// </summary>
    [GraphQLDescription("Actualiza un log de microservicio existente en FastServer_Logs (PostgreSQL)")]
    public async Task<LogMicroserviceDto> UpdateLogMicroservice(
        [Service] ILogMicroserviceService service,
        [GraphQLDescription("Datos a actualizar")] UpdateLogMicroserviceInput input,
        CancellationToken cancellationToken = default)
    {
        var dto = new UpdateLogMicroserviceDto
        {
            LogMicroserviceId = input.LogMicroserviceId,
            EventName = input.EventName,
            LogDate = input.LogDate,
            LogLevel = input.LogLevel,
            LogMicroserviceText = input.LogMicroserviceText
        };

        return await service.UpdateAsync(dto, cancellationToken);
    }

    /// <summary>
    /// Actualiza múltiples logs de microservicio en una sola operación
    /// </summary>
    [GraphQLDescription("Actualiza múltiples logs de microservicio en una sola operación atómica en FastServer_Logs (PostgreSQL)")]
    public async Task<BulkUpdateResultDto<LogMicroserviceDto>> BulkUpdateLogMicroservice(
        [Service] ILogMicroserviceService service,
        [GraphQLDescription("Datos de los logs a actualizar")] BulkUpdateLogMicroserviceInput input,
        CancellationToken cancellationToken = default)
    {
        var dtos = input.Items.Select(i => new UpdateLogMicroserviceDto
        {
            LogMicroserviceId = i.LogMicroserviceId,
            EventName = i.EventName,
            LogDate = i.LogDate,
            LogLevel = i.LogLevel,
            LogMicroserviceText = i.LogMicroserviceText
        });

        return await service.UpdateBulkAsync(dtos, cancellationToken);
    }

    /// <summary>
    /// Elimina un log de microservicio
    /// </summary>
    [GraphQLDescription("Elimina un log de microservicio por su ID desde FastServer_Logs (PostgreSQL)")]
    public async Task<bool> DeleteLogMicroservice(
        [Service] ILogMicroserviceService service,
        [GraphQLDescription("ID del log a eliminar")] long logId,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(logId, cancellationToken);
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
    [GraphQLDescription("Crea un nuevo contenido de log de servicios en FastServer_Logs (PostgreSQL)")]
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

        return await service.CreateAsync(dto, cancellationToken);
    }

    /// <summary>
    /// Crea múltiples contenidos de log en una sola operación
    /// </summary>
    [GraphQLDescription("Crea múltiples contenidos de log en una sola operación atómica en FastServer_Logs (PostgreSQL)")]
    public async Task<BulkInsertResultDto<LogServicesContentDto>> BulkCreateLogServicesContent(
        [Service] ILogServicesContentService service,
        [GraphQLDescription("Datos de los contenidos a crear")] BulkCreateLogServicesContentInput input,
        CancellationToken cancellationToken = default)
    {
        var dtos = input.Items.Select(i => new CreateLogServicesContentDto
        {
            LogId = i.LogId,
            LogServicesDate = i.LogServicesDate,
            LogServicesLogLevel = i.LogServicesLogLevel,
            LogServicesState = i.LogServicesState,
            LogServicesContentText = i.LogServicesContentText
        });

        return await service.CreateBulkAsync(dtos, cancellationToken);
    }

    /// <summary>
    /// Actualiza un contenido de log
    /// </summary>
    [GraphQLDescription("Actualiza un contenido de log existente en FastServer_Logs (PostgreSQL)")]
    public async Task<LogServicesContentDto> UpdateLogServicesContent(
        [Service] ILogServicesContentService service,
        [GraphQLDescription("Datos a actualizar")] UpdateLogServicesContentInput input,
        CancellationToken cancellationToken = default)
    {
        var dto = new UpdateLogServicesContentDto
        {
            LogId = input.LogId,
            LogServicesDate = input.LogServicesDate,
            LogServicesLogLevel = input.LogServicesLogLevel,
            LogServicesState = input.LogServicesState,
            LogServicesContentText = input.LogServicesContentText
        };

        return await service.UpdateAsync(dto, cancellationToken);
    }

    /// <summary>
    /// Actualiza múltiples contenidos de log en una sola operación
    /// </summary>
    [GraphQLDescription("Actualiza múltiples contenidos de log en una sola operación atómica en FastServer_Logs (PostgreSQL)")]
    public async Task<BulkUpdateResultDto<LogServicesContentDto>> BulkUpdateLogServicesContent(
        [Service] ILogServicesContentService service,
        [GraphQLDescription("Datos de los contenidos a actualizar")] BulkUpdateLogServicesContentInput input,
        CancellationToken cancellationToken = default)
    {
        var dtos = input.Items.Select(i => new UpdateLogServicesContentDto
        {
            LogId = i.LogId,
            LogServicesDate = i.LogServicesDate,
            LogServicesLogLevel = i.LogServicesLogLevel,
            LogServicesState = i.LogServicesState,
            LogServicesContentText = i.LogServicesContentText
        });

        return await service.UpdateBulkAsync(dtos, cancellationToken);
    }

    /// <summary>
    /// Elimina un contenido de log
    /// </summary>
    [GraphQLDescription("Elimina un contenido de log por su ID desde FastServer_Logs (PostgreSQL)")]
    public async Task<bool> DeleteLogServicesContent(
        [Service] ILogServicesContentService service,
        [GraphQLDescription("ID del log a eliminar")] long logId,
        CancellationToken cancellationToken = default)
    {
        return await service.DeleteAsync(logId, cancellationToken);
    }
}
