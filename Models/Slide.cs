namespace MyPresentationApp.Models;

public class Slide
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? PresentationId { get; set; }
    public int Order { get; set; } 
    public string? Background { get; set; } 
    public AnimationType? Animation { get; set; }
    
    public Presentation? Presentation { get; set; }
    public ICollection<SlideElement> Elements { get; set; } = new List<SlideElement>();
}

public enum AnimationType
{
    None,
    Fade,
    Slide,
    Zoom
}



public class SlideDto
{
    public string Id { get; set; }
    public int Order { get; set; }
    public string Background { get; set; }
}