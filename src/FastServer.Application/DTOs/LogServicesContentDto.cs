namespace FastServer.Application.DTOs;

/// <summary>
/// DTO para LogServicesContent
/// </summary>
public record LogServicesContentDto
{
    public long LogId { get; init; }
    public string? LogServicesDate { get; init; }
    public string? LogServicesLogLevel { get; init; }
    public string? LogServicesState { get; init; }
    public string? LogServicesContentText { get; init; }
}

/// <summary>
/// DTO para crear LogServicesContent
/// </summary>
public record CreateLogServicesContentDto
{
    public long LogId { get; init; }
    public string? LogServicesDate { get; init; }
    public string? LogServicesLogLevel { get; init; }
    public string? LogServicesState { get; init; }
    public string? LogServicesContentText { get; init; }
}

/// <summary>
/// DTO para actualizar LogServicesContent
/// </summary>
public record UpdateLogServicesContentDto
{
    public long LogId { get; init; }
    public string? LogServicesDate { get; init; }
    public string? LogServicesLogLevel { get; init; }
    public string? LogServicesState { get; init; }
    public string? LogServicesContentText { get; init; }
}
