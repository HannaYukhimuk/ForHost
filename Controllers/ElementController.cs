using Microsoft.AspNetCore.Mvc;
using MyPresentationApp.Models;
using MyPresentationApp.Data;
using MyPresentationApp.Services;
using MyPresentationApp.Models.DTOs;

namespace MyPresentationApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ElementsController : ControllerBase
{
    private readonly ElementService _elementService;
    private readonly AppDbContext _db;

    public ElementsController(AppDbContext db, ElementService elementService)
    {
        _db = db;
        _elementService = elementService;
    }

    [HttpPost("text")]
    public async Task<IActionResult> AddTextElement([FromBody] AddTextElementRequest request)
    {
        try
        {
            var element = await _elementService.AddTextElement(request);
            return Ok(element);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("shape")]
    public async Task<IActionResult> AddShapeElement([FromBody] AddShapeElementRequest request)
    {
        try
        {
            var element = await _elementService.AddShapeElement(request);
            
            return Ok(new
            {
                id = element.Id,
                type = element.Type.ToString(),
                fillColor = element.FillColor,
                borderColor = element.BorderColor,
                borderWidth = element.BorderWidth,
                positionX = element.PositionX,
                positionY = element.PositionY,
                width = element.Width,
                height = element.Height
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPut("position")]
    public async Task<IActionResult> SaveElementPosition([FromBody] SaveElementPositionRequest request)
    {
        try
        {
            await _elementService.UpdateElementPosition(request);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{elementId}")]
    public async Task<IActionResult> DeleteElement(string elementId)
    {
        try
        {
            await _elementService.DeleteElement(elementId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("style")]
    public async Task<IActionResult> SaveElementStyle([FromBody] SaveElementStyleRequest request)
    {
        try
        {
            await _elementService.UpdateElementStyle(request);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("zindex")]
    public async Task<IActionResult> SaveElementZIndex([FromBody] SaveElementZIndexRequest request)
    {
        try
        {
            await _elementService.UpdateElementZIndex(request);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("shape/style")]
    public async Task<IActionResult> SaveShapeStyle([FromBody] SaveShapeStyleRequest request)
    {
        try
        {
            await _elementService.UpdateShapeStyle(request);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}