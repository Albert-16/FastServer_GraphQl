namespace FastServer.Domain.Enums;

/// <summary>
/// Estado del log de servicio
/// </summary>
public enum LogState
{
    Pending = 0,
    InProgress = 1,
    Completed = 2,
    Failed = 3,
    Timeout = 4,
    Cancelled = 5
}
