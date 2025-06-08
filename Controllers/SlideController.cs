using Microsoft.AspNetCore.Mvc;
using MyPresentationApp.Models;
using MyPresentationApp.Data;
using MyPresentationApp.Services;

namespace MyPresentationApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SlidesController : ControllerBase
{
    private readonly SlideService _slideService;
    private readonly AppDbContext _db;

    public SlidesController(AppDbContext db, SlideService slideService)
    {
        _db = db;
        _slideService = slideService;
    }


    [HttpPost("{presentationId}")]
    public async Task<IActionResult> AddSlide(string presentationId)
    {
        try
        {
            var slide = await _slideService.AddSlideToPresentation(presentationId);
            return Ok(new SlideDto {
                Id = slide.Id,
                Order = slide.Order,
                Background = slide.Background
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpDelete("{slideId}")]
    public async Task<IActionResult> DeleteSlide(string slideId)
    {
        var result = await _slideService.DeleteSlide(slideId);
        if (!result) return NotFound();
        return Ok();
    }

    [HttpGet("{slideId}/elements")]
    public async Task<IActionResult> GetSlideElements(string slideId)
    {
        try
        {
            var slide = await _slideService.GetSlideWithElements(slideId);
            if (slide == null) return NotFound();

            return Ok(new
            {
                elements = slide.Elements.Select(e => new
                {
                    id = e.Id,
                    type = e.Type.ToString(),
                    content = e.Content,
                    color = e.Color,
                    fillColor = e.FillColor,
                    borderColor = e.BorderColor,
                    borderWidth = e.BorderWidth,
                    positionX = e.PositionX,
                    positionY = e.PositionY,
                    width = e.Width,
                    height = e.Height,
                    fontSize = e.FontSize,
                    fontStyle = e.FontStyle,
                    fontWeight = e.FontWeight,
                    zIndex = e.ZIndex
                }),
                slide = new
                {
                    id = slide.Id,
                    order = slide.Order,
                    background = slide.Background
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}