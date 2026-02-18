namespace FastServer.Application.DTOs;

/// <summary>
/// DTO para LogMicroservice
/// </summary>
public record LogMicroserviceDto
{
    public Guid LogMicroserviceId { get; init; }
    public long LogId { get; init; }
    public long RequestId { get; init; }
    public string EventName { get; init; } = string.Empty;
    public DateTime? LogDate { get; init; }
    public string? LogLevel { get; init; }
    public string? LogMicroserviceText { get; init; }
}

/// <summary>
/// DTO para crear LogMicroservice
/// </summary>
public record CreateLogMicroserviceDto
{
    public long LogId { get; init; }
    public long RequestId { get; init; }
    public string EventName { get; init; } = string.Empty;
    public DateTime? LogDate { get; init; }
    public string? LogLevel { get; init; }
    public string? LogMicroserviceText { get; init; }
}

/// <summary>
/// DTO para actualizar LogMicroservice
/// </summary>
public record UpdateLogMicroserviceDto
{
    public Guid LogMicroserviceId { get; init; }
    public string? EventName { get; init; }
    public DateTime? LogDate { get; init; }
    public string? LogLevel { get; init; }
    public string? LogMicroserviceText { get; init; }
}
