using Microsoft.EntityFrameworkCore;
using MyPresentationApp.Data;
using MyPresentationApp.Models;

namespace MyPresentationApp.Services;

public class SlideService
{
    private readonly AppDbContext _db;

    public SlideService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Slide> AddSlideToPresentation(string presentationId)
    {
        var presentation = await LoadPresentation(presentationId);
        var slide = CreateNewSlide(presentation);
        _db.Slides.Add(slide);
        await _db.SaveChangesAsync();
        return slide;
    }

    private async Task<Presentation> LoadPresentation(string id)
    {
        var presentation = await _db.Presentations
            .Include(p => p.Slides)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (presentation == null)
            throw new ArgumentException($"Presentation with ID {id} not found");

        return presentation;
    }

    private Slide CreateNewSlide(Presentation presentation) =>
        new()
        {
            PresentationId = presentation.Id,
            Order = presentation.Slides.Count + 1,
            Background = "#ffffff"
        };

    public async Task<bool> DeleteSlide(string slideId)
    {
        var slide = await LoadSlideWithElements(slideId);
        if (slide == null) return false;

        DeleteSlideAndElements(slide);
        await _db.SaveChangesAsync();

        await ReorderSlides(slide.PresentationId);
        return true;
    }

    private async Task<Slide?> LoadSlideWithElements(string id) =>
        await _db.Slides.Include(s => s.Elements).FirstOrDefaultAsync(s => s.Id == id);

    private void DeleteSlideAndElements(Slide slide)
    {
        _db.SlideElements.RemoveRange(slide.Elements);
        _db.Slides.Remove(slide);
    }

    public async Task<Slide> GetSlideWithElements(string slideId) =>
        await _db.Slides.Include(s => s.Elements).FirstOrDefaultAsync(s => s.Id == slideId);

    public async Task ReorderSlides(string presentationId)
    {
        var slides = await GetSlidesOrdered(presentationId);
        RenumberSlides(slides);
        await _db.SaveChangesAsync();
    }

    private async Task<List<Slide>> GetSlidesOrdered(string presentationId) =>
        await _db.Slides
            .Where(s => s.PresentationId == presentationId)
            .OrderBy(s => s.Order)
            .ToListAsync();

    private void RenumberSlides(List<Slide> slides)
    {
        for (int i = 0; i < slides.Count; i++)
            slides[i].Order = i + 1;
    }
}
