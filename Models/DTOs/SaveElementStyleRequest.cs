namespace MyPresentationApp.Models.DTOs;

public class SaveElementStyleRequest
{
    public string? ElementId { get; set; }
    public int FontSize { get; set; }
    public string? FontStyle { get; set; }
    public string? FontWeight { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}