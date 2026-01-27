namespace FastServer.Application.DTOs.Microservices;

/// <summary>
/// DTO para ActivityLog
/// </summary>
public class ActivityLogDto
{
    public Guid ActivityLogId { get; set; }
    public long? EventTypeId { get; set; }
    public string? ActivityLogEntityName { get; set; }
    public Guid? ActivityLogEntityId { get; set; }
    public string? ActivityLogDescription { get; set; }
    public Guid? UserId { get; set; }
    public DateTime? CreateAt { get; set; }
    public DateTime? ModifyAt { get; set; }

    // Relaciones
    public EventTypeDto? EventType { get; set; }
    public UserDto? User { get; set; }
}
