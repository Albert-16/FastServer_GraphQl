using FastServer.Domain.Enums;

namespace FastServer.Application.DTOs;

/// <summary>
/// DTO para filtrar logs
/// </summary>
public record LogFilterDto
{
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public LogState? State { get; init; }
    public string? MicroserviceName { get; init; }
    public string? UserId { get; init; }
    public string? TransactionId { get; init; }
    public string? HttpMethod { get; init; }
    public bool? HasErrors { get; init; }
    public DataSourceType? DataSource { get; init; }
}
