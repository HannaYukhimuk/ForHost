namespace MyPresentationApp.Models;

public class Presentation
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = "New Presentation";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedById { get; set; } 
    
    public User? CreatedBy { get; set; }
    public ICollection<Slide> Slides { get; set; } = new List<Slide>();
    public ICollection<PresentationUser>? Users { get; set; } 
    public string? CurrentSlideId { get; set; } 
}

