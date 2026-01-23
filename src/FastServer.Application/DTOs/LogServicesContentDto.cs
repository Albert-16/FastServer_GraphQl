namespace FastServer.Application.DTOs;

/// <summary>
/// DTO para LogServicesContent
/// </summary>
public record LogServicesContentDto
{
    public long LogId { get; init; }
    public string? LogServicesContentText { get; init; }
    public string? ContentNo { get; init; }
}

/// <summary>
/// DTO para crear LogServicesContent
/// </summary>
public record CreateLogServicesContentDto
{
    public long LogId { get; init; }
    public string? LogServicesContentText { get; init; }
    public string? ContentNo { get; init; }
}
