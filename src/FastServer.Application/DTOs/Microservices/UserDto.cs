namespace FastServer.Application.DTOs.Microservices;

/// <summary>
/// DTO para User
/// </summary>
public class UserDto
{
    public Guid UserId { get; set; }
    public string? UserPeoplesoft { get; set; }
    public bool? UserActive { get; set; }
    public string? UserName { get; set; }
    public string? UserEmail { get; set; }
    public DateTime? CreateAt { get; set; }
    public DateTime? ModifyAt { get; set; }
}
