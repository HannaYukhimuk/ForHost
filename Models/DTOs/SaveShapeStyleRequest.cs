namespace MyPresentationApp.Models.DTOs;

public class SaveShapeStyleRequest
{
    public string? ElementId { get; set; }
    public string? FillColor { get; set; }
    public string? BorderColor { get; set; }
}