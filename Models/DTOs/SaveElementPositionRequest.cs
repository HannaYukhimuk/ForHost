namespace MyPresentationApp.Models.DTOs;

public class SaveElementPositionRequest
{
    public string? ElementId { get; set; }
    public int PositionX { get; set; }
    public int PositionY { get; set; }
}
