using FastServer.Domain.Enums;

namespace FastServer.Application.Events.LogEvents;

/// <summary>
/// Evento publicado cuando se actualiza un log
/// </summary>
public class LogUpdatedEvent
{
    public long LogId { get; set; }
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
    public DateTime UpdatedAt { get; set; }
}
