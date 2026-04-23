using FastServer.Domain.Entities;

namespace FastServer.GraphQL.Api.GraphQL.Types;

/// <summary>
/// Tipo GraphQL para LogServicesHeaderHistorico (entidad histórica inmutable).
/// </summary>
public class LogServicesHeaderHistoricoType : ObjectType<LogServicesHeaderHistorico>
{
    protected override void Configure(IObjectTypeDescriptor<LogServicesHeaderHistorico> descriptor)
    {
        descriptor.Name("LogServicesHeaderHistorico");
        descriptor.Description("Cabecera histórica de logs de servicios FastServer (archivo inmutable)");

        descriptor.Field(x => x.LogId)
            .Name("logId")
            .Description("Identificador único del log");

        descriptor.Field(x => x.LogDateIn)
            .Name("logDateIn")
            .Description("Fecha y hora de entrada del log");

        descriptor.Field(x => x.LogDateOut)
            .Name("logDateOut")
            .Description("Fecha y hora de salida del log");

        descriptor.Field(x => x.LogState)
            .Name("logState")
            .Description("Estado del log");

        descriptor.Field(x => x.LogMethodUrl)
            .Name("logMethodUrl")
            .Description("URL del método invocado");

        descriptor.Field(x => x.LogMethodName)
            .Name("logMethodName")
            .Description("Nombre del método invocado");

        descriptor.Field(x => x.LogFsId)
            .Name("logFsId")
            .Description("ID de FastServer");

        descriptor.Field(x => x.MethodDescription)
            .Name("methodDescription")
            .Description("Descripción del método");

        descriptor.Field(x => x.TciIpPort)
            .Name("tciIpPort")
            .Description("IP y puerto TCI");

        descriptor.Field(x => x.ErrorCode)
            .Name("errorCode")
            .Description("Código de error");

        descriptor.Field(x => x.ErrorDescription)
            .Name("errorDescription")
            .Description("Descripción del error");

        descriptor.Field(x => x.IpFs)
            .Name("ipFs")
            .Description("IP del FastServer");

        descriptor.Field(x => x.TypeProcess)
            .Name("typeProcess")
            .Description("Tipo de proceso");

        descriptor.Field(x => x.LogNodo)
            .Name("logNodo")
            .Description("Nodo del log");

        descriptor.Field(x => x.HttpMethod)
            .Name("httpMethod")
            .Description("Método HTTP");

        descriptor.Field(x => x.MicroserviceName)
            .Name("microserviceName")
            .Description("Nombre del microservicio");

        descriptor.Field(x => x.RequestDuration)
            .Name("requestDuration")
            .Description("Duración de la solicitud (ej: 1980.67 ms)");

        descriptor.Field(x => x.TransactionId)
            .Name("transactionId")
            .Description("ID de la transacción");

        descriptor.Field(x => x.UserId)
            .Name("userId")
            .Description("ID del usuario");

        descriptor.Field(x => x.SessionId)
            .Name("sessionId")
            .Description("ID de la sesión");
    }
}

/// <summary>
/// Tipo GraphQL para LogServicesContentHistorico (entidad histórica inmutable).
/// </summary>
public class LogServicesContentHistoricoType : ObjectType<LogServicesContentHistorico>
{
    protected override void Configure(IObjectTypeDescriptor<LogServicesContentHistorico> descriptor)
    {
        descriptor.Name("LogServicesContentHistorico");
        descriptor.Description("Contenido histórico de log de servicios (archivo inmutable)");

        descriptor.Field(x => x.LogServicesContentId)
            .Name("logServicesContentId")
            .Description("Identificador único del registro (GUID v7)");

        descriptor.Field(x => x.LogId)
            .Name("logId")
            .Description("Identificador del log");

        descriptor.Field(x => x.EventName)
            .Name("eventName")
            .Description("Nombre del evento");

        descriptor.Field(x => x.LogServicesDate)
            .Name("logServicesDate")
            .Description("Fecha del log de servicio");

        descriptor.Field(x => x.LogServicesLogLevel)
            .Name("logServicesLogLevel")
            .Description("Nivel del log");

        descriptor.Field(x => x.LogServicesState)
            .Name("logServicesState")
            .Description("Estado del log de servicio");

        descriptor.Field(x => x.LogServicesContentText)
            .Name("logServicesContentText")
            .Description("Texto del contenido del log");
    }
}
