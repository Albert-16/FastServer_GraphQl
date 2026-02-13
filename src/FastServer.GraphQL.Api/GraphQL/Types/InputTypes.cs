using FastServer.Domain.Enums;

namespace FastServer.GraphQL.Api.GraphQL.Types;

/// <summary>
/// Input para crear LogServicesHeader
/// </summary>
public class CreateLogServicesHeaderInput
{
    public DateTime LogDateIn { get; set; }
    public DateTime LogDateOut { get; set; }
    public LogState LogState { get; set; }
    public string LogMethodUrl { get; set; } = string.Empty;
    public string? LogMethodName { get; set; }
    public long? LogFsId { get; set; }
    public string? MethodDescription { get; set; }
    public string? TciIpPort { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorDescription { get; set; }
    public string? IpFs { get; set; }
    public string? TypeProcess { get; set; }
    public string? LogNodo { get; set; }
    public string? HttpMethod { get; set; }
    public string? MicroserviceName { get; set; }
    public long? RequestDuration { get; set; }
    public string? TransactionId { get; set; }
    public string? UserId { get; set; }
    public string? SessionId { get; set; }
    public long? RequestId { get; set; }
}

/// <summary>
/// Input para actualizar LogServicesHeader
/// </summary>
public class UpdateLogServicesHeaderInput
{
    public long LogId { get; set; }
    public DateTime? LogDateOut { get; set; }
    public LogState? LogState { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorDescription { get; set; }
    public long? RequestDuration { get; set; }
}

/// <summary>
/// Input para crear LogMicroservice
/// </summary>
public class CreateLogMicroserviceInput
{
    public long LogId { get; set; }
    public DateTime? LogDate { get; set; }
    public string? LogLevel { get; set; }
    public string? LogMicroserviceText { get; set; }
}

/// <summary>
/// Input para crear LogServicesContent
/// </summary>
public class CreateLogServicesContentInput
{
    public long LogId { get; set; }
    public string? LogServicesDate { get; set; }
    public string? LogServicesLogLevel { get; set; }
    public string? LogServicesState { get; set; }
    public string? LogServicesContentText { get; set; }
}

/// <summary>
/// Input para inserción masiva de LogServicesHeader
/// </summary>
public class BulkCreateLogServicesHeaderInput
{
    [GraphQLDescription("Lista de logs de servicios a crear (máximo 1000)")]
    public List<CreateLogServicesHeaderInput> Items { get; set; } = new();
}

/// <summary>
/// Input para inserción masiva de LogMicroservice
/// </summary>
public class BulkCreateLogMicroserviceInput
{
    [GraphQLDescription("Lista de logs de microservicio a crear (máximo 1000)")]
    public List<CreateLogMicroserviceInput> Items { get; set; } = new();
}

/// <summary>
/// Input para filtrar logs
/// </summary>
public class LogFilterInput
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public LogState? State { get; set; }
    public string? MicroserviceName { get; set; }
    public string? UserId { get; set; }
    public string? TransactionId { get; set; }
    public string? HttpMethod { get; set; }
    public bool? HasErrors { get; set; }
}

/// <summary>
/// Input para paginación
/// </summary>
public class PaginationInput
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

/// <summary>
/// Tipos de input para GraphQL
/// </summary>
public class CreateLogServicesHeaderInputType : InputObjectType<CreateLogServicesHeaderInput>
{
    protected override void Configure(IInputObjectTypeDescriptor<CreateLogServicesHeaderInput> descriptor)
    {
        descriptor.Name("CreateLogServicesHeaderInput");
        descriptor.Description("Input para crear un nuevo log de servicios");
    }
}

public class UpdateLogServicesHeaderInputType : InputObjectType<UpdateLogServicesHeaderInput>
{
    protected override void Configure(IInputObjectTypeDescriptor<UpdateLogServicesHeaderInput> descriptor)
    {
        descriptor.Name("UpdateLogServicesHeaderInput");
        descriptor.Description("Input para actualizar un log de servicios");
    }
}

public class CreateLogMicroserviceInputType : InputObjectType<CreateLogMicroserviceInput>
{
    protected override void Configure(IInputObjectTypeDescriptor<CreateLogMicroserviceInput> descriptor)
    {
        descriptor.Name("CreateLogMicroserviceInput");
        descriptor.Description("Input para crear un log de microservicio");
    }
}

public class CreateLogServicesContentInputType : InputObjectType<CreateLogServicesContentInput>
{
    protected override void Configure(IInputObjectTypeDescriptor<CreateLogServicesContentInput> descriptor)
    {
        descriptor.Name("CreateLogServicesContentInput");
        descriptor.Description("Input para crear contenido de log");
    }
}

public class LogFilterInputType : InputObjectType<LogFilterInput>
{
    protected override void Configure(IInputObjectTypeDescriptor<LogFilterInput> descriptor)
    {
        descriptor.Name("LogFilterInput");
        descriptor.Description("Input para filtrar logs");
    }
}

public class PaginationInputType : InputObjectType<PaginationInput>
{
    protected override void Configure(IInputObjectTypeDescriptor<PaginationInput> descriptor)
    {
        descriptor.Name("PaginationInput");
        descriptor.Description("Input para paginación");
    }
}

public class BulkCreateLogServicesHeaderInputType : InputObjectType<BulkCreateLogServicesHeaderInput>
{
    protected override void Configure(IInputObjectTypeDescriptor<BulkCreateLogServicesHeaderInput> descriptor)
    {
        descriptor.Name("BulkCreateLogServicesHeaderInput");
        descriptor.Description("Input para inserción masiva de logs de servicios");
    }
}

public class BulkCreateLogMicroserviceInputType : InputObjectType<BulkCreateLogMicroserviceInput>
{
    protected override void Configure(IInputObjectTypeDescriptor<BulkCreateLogMicroserviceInput> descriptor)
    {
        descriptor.Name("BulkCreateLogMicroserviceInput");
        descriptor.Description("Input para inserción masiva de logs de microservicio");
    }
}
