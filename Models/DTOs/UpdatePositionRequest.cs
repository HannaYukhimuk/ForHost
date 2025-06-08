namespace MyPresentationApp.Models.DTOs;

public class UpdatePositionRequest
{
    public string? ElementId { get; set; }
    public int PositionX { get; set; }
    public int PositionY { get; set; }
}