namespace MyPresentationApp.Models;

public class PresentationUser
{
    public string? UserId { get; set; }
    public string? PresentationId { get; set; }
    public UserRole Role { get; set; } 

    public User? User { get; set; }
    public Presentation? Presentation { get; set; }
}

public enum UserRole
{
    Viewer,
    Editor,
    Creator
}