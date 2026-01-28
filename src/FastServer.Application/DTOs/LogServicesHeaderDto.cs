using FastServer.Domain.Enums;

namespace FastServer.Application.DTOs;

/// <summary>
/// DTO para LogServicesHeader
/// </summary>
public record LogServicesHeaderDto
{
    public long LogId { get; init; }
    public DateTime LogDateIn { get; init; }
    public DateTime LogDateOut { get; init; }
    public LogState LogState { get; init; }
    public string LogMethodUrl { get; init; } = string.Empty;
    public string? LogMethodName { get; init; }
    public long? LogFsId { get; init; }
    public string? MethodDescription { get; init; }
    public string? TciIpPort { get; init; }
    public string? ErrorCode { get; init; }
    public string? ErrorDescription { get; init; }
    public string? IpFs { get; init; }
    public string? TypeProcess { get; init; }
    public string? LogNodo { get; init; }
    public string? HttpMethod { get; init; }
    public string? MicroserviceName { get; init; }
    public long? RequestDuration { get; init; }
    public string? TransactionId { get; init; }
    public string? UserId { get; init; }
    public string? SessionId { get; init; }
    public long? RequestId { get; init; }
}

/// <summary>
/// DTO para crear LogServicesHeader
/// </summary>
public record CreateLogServicesHeaderDto
{
    public DateTime LogDateIn { get; init; }
    public DateTime LogDateOut { get; init; }
    public LogState LogState { get; init; }
    public string LogMethodUrl { get; init; } = string.Empty;
    public string? LogMethodName { get; init; }
    public long? LogFsId { get; init; }
    public string? MethodDescription { get; init; }
    public string? TciIpPort { get; init; }
    public string? ErrorCode { get; init; }
    public string? ErrorDescription { get; init; }
    public string? IpFs { get; init; }
    public string? TypeProcess { get; init; }
    public string? LogNodo { get; init; }
    public string? HttpMethod { get; init; }
    public string? MicroserviceName { get; init; }
    public long? RequestDuration { get; init; }
    public string? TransactionId { get; init; }
    public string? UserId { get; init; }
    public string? SessionId { get; init; }
    public long? RequestId { get; init; }
}

/// <summary>
/// DTO para actualizar LogServicesHeader
/// </summary>
public record UpdateLogServicesHeaderDto
{
    public long LogId { get; init; }
    public DateTime? LogDateOut { get; init; }
    public LogState? LogState { get; init; }
    public string? ErrorCode { get; init; }
    public string? ErrorDescription { get; init; }
    public long? RequestDuration { get; init; }
}
