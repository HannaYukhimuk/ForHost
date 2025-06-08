using MyPresentationApp.Data;
using MyPresentationApp.Models;
using MyPresentationApp.Models.DTOs;

namespace MyPresentationApp.Services;

public class ElementService
{
    private readonly AppDbContext _db;

    public ElementService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<SlideElement> AddTextElement(AddTextElementRequest request)
    {
        var element = new SlideElement
        {
            SlideId = request.SlideId,
            Type = ElementType.Text,
            Content = request.Content,
            ZIndex = 0,
            Color = request.Color,
            PositionX = 100,
            PositionY = 100,
            Width = request.Width,
            Height = request.Height,
            FontSize = request.FontSize,
            FontStyle = request.FontStyle,
            FontWeight = request.FontWeight
        };

        _db.SlideElements.Add(element);
        await _db.SaveChangesAsync();

        return element;
    }

    public async Task<SlideElement> AddShapeElement(AddShapeElementRequest request)
    {
        var shapeType = (request.ShapeType ?? "rectangle").ToLower();
        
        var elementType = shapeType switch
        {
            "circle" => ElementType.Circle,
            "triangle" => ElementType.Triangle,
            _ => ElementType.Rectangle
        };

        var element = new SlideElement
        {
            SlideId = request.SlideId,
            Type = elementType,
            ZIndex = 0,
            Content = string.Empty,
            FillColor = request.FillColor,
            BorderColor = request.BorderColor,
            BorderWidth = request.BorderWidth,
            PositionX = 100,
            PositionY = 100,
            Width = request.Width,
            Height = request.Height
        };

        _db.SlideElements.Add(element);
        await _db.SaveChangesAsync();

        return element;
    }

    public async Task UpdateElementPosition(SaveElementPositionRequest request)
    {
        var element = await _db.SlideElements.FindAsync(request.ElementId);
        if (element == null) throw new ArgumentException("Element not found");

        element.PositionX = request.PositionX;
        element.PositionY = request.PositionY;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteElement(string elementId)
    {
        var element = await _db.SlideElements.FindAsync(elementId);
        if (element == null) throw new ArgumentException("Element not found");

        _db.SlideElements.Remove(element);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateElementStyle(SaveElementStyleRequest request)
    {
        var element = await _db.SlideElements.FindAsync(request.ElementId);
        if (element == null) throw new ArgumentException("Element not found");

        element.FontSize = request.FontSize;
        element.FontStyle = request.FontStyle;
        element.FontWeight = request.FontWeight;
        element.Width = request.Width;
        element.Height = request.Height;
        await _db.SaveChangesAsync();
    }

    public async Task UpdateElementZIndex(SaveElementZIndexRequest request)
    {
        var element = await _db.SlideElements.FindAsync(request.ElementId);
        if (element == null) throw new ArgumentException("Element not found");

        element.ZIndex = request.ZIndex;
        await _db.SaveChangesAsync();
    }

    public async Task UpdateShapeStyle(SaveShapeStyleRequest request)
    {
        var element = await _db.SlideElements.FindAsync(request.ElementId);
        if (element == null) throw new ArgumentException("Element not found");

        element.FillColor = request.FillColor;
        element.BorderColor = request.BorderColor;
        await _db.SaveChangesAsync();
    }
}