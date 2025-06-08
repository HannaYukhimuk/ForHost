namespace MyPresentationApp.Models.DTOs;

public class AddTextElementRequest
{
    public string? SlideId { get; set; }
    public string? Content { get; set; }
    public string Color { get; set; } = "#000000";
    public int Width { get; set; } = 200;  // Добавляем свойства
    public int Height { get; set; } = 50;
    public int FontSize { get; set; } = 16;
    public string FontStyle { get; set; } = "normal";
    public string FontWeight { get; set; } = "normal";
}