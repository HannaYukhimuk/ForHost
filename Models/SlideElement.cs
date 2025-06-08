namespace MyPresentationApp.Models;

public class SlideElement
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string SlideId { get; set; }
    public ElementType Type { get; set; }
    public int ZIndex { get; set; } = 0;
    public string? Content { get; set; }
    public string Color { get; set; } = "#000000";
    public string FillColor { get; set; } = "#ffffff"; 
    public string BorderColor { get; set; } = "#000000"; 
    public int BorderWidth { get; set; } = 1; 
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public int Width { get; set; } = 200;
    public int Height { get; set; } = 50;
    public string FontStyle { get; set; } = "normal";
    public string FontWeight { get; set; } = "normal";
    public int FontSize { get; set; } = 16;

    public Slide? Slide { get; set; }
}




public enum ElementType
{
    Text,
    Rectangle,
    Circle,
    Triangle,
    Image,
    Arrow
}