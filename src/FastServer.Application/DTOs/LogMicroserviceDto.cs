namespace FastServer.Application.DTOs;

/// <summary>
/// DTO para LogMicroservice
/// </summary>
public record LogMicroserviceDto
{
    public long LogId { get; init; }
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
    public DateTime? LogDate { get; init; }
    public string? LogLevel { get; init; }
    public string? LogMicroserviceText { get; init; }
}
