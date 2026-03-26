namespace FastServer.Application.DTOs;

/// <summary>
/// DTO para resultados de inserción masiva
/// </summary>
public record BulkInsertResultDto<T>
{
    public IEnumerable<T> InsertedItems { get; init; } = Enumerable.Empty<T>();
    public int TotalRequested { get; init; }
    public int TotalInserted { get; init; }
    public int TotalFailed { get; init; }
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public IEnumerable<BulkInsertError> Errors { get; init; } = Enumerable.Empty<BulkInsertError>();
}

/// <summary>
/// Error individual en inserción masiva
/// </summary>
public record BulkInsertError
{
    public int Index { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;
    public object? FailedItem { get; init; }
}

/// <summary>
/// DTO para resultados de actualización masiva
/// </summary>
public record BulkUpdateResultDto<T>
{
    public IEnumerable<T> UpdatedItems { get; init; } = Enumerable.Empty<T>();
    public int TotalRequested { get; init; }
    public int TotalUpdated { get; init; }
    public int TotalFailed { get; init; }
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public IEnumerable<BulkUpdateError> Errors { get; init; } = Enumerable.Empty<BulkUpdateError>();
}

/// <summary>
/// Error individual en actualización masiva
/// </summary>
public record BulkUpdateError
{
    public int Index { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;
    public object? FailedItem { get; init; }
}

