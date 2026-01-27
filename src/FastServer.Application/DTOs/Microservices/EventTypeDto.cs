namespace FastServer.Application.DTOs.Microservices;

/// <summary>
/// DTO para EventType
/// </summary>
public class EventTypeDto
{
    public long EventTypeId { get; set; }
    public string? EventTypeDescription { get; set; }
    public DateTime? CreateAt { get; set; }
    public DateTime? ModifyAt { get; set; }
}
