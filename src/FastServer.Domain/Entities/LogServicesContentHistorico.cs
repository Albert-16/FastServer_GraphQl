namespace FastServer.Domain.Entities;

/// <summary>
/// Histórico de contenido de logs de servicios - FastServer_LogServices_Content_Historico
/// </summary>
public class LogServicesContentHistorico : BaseEntity
{
    public string? LogServicesContentText { get; set; }
    public string ContentNo { get; set; } = string.Empty;

    public virtual LogServicesHeaderHistorico? LogServicesHeader { get; set; }
}
