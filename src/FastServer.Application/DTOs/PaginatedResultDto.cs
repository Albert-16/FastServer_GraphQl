namespace FastServer.Application.DTOs;

/// <summary>
/// DTO para resultados paginados
/// </summary>
public record PaginatedResultDto<T>
{
    public IEnumerable<T> Items { get; init; } = Enumerable.Empty<T>();
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

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
/// DTO para parámetros de paginación
/// </summary>
public record PaginationParamsDto
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public int Skip => (PageNumber - 1) * PageSize;
}
