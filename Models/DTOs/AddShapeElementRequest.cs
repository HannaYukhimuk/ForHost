namespace MyPresentationApp.Models.DTOs;

public class AddShapeElementRequest
{
    public string? SlideId { get; set; }
    public string? ShapeType { get; set; } // "rectangle", "circle", "triangle"
    public string FillColor { get; set; } = "#ffffff";
    public string BorderColor { get; set; } = "#000000";
    public int BorderWidth { get; set; } = 1;
    public int Width { get; set; } = 100;
    public int Height { get; set; } = 100;
}



