namespace FastServer.Domain.Entities;

/// <summary>
/// Histórico de logs de microservicios - FastServer_LogMicroservice_Historico
/// </summary>
public class LogMicroserviceHistorico : BaseEntity
{
    public string LogMicroserviceText { get; set; } = string.Empty;

    public virtual LogServicesHeaderHistorico? LogServicesHeader { get; set; }
}
