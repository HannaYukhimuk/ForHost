using Microsoft.EntityFrameworkCore;
using MyPresentationApp.Data;
using MyPresentationApp.Models;

namespace MyPresentationApp.Services;

public class PresentationService
{
    private readonly AppDbContext _db;

    public PresentationService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Presentation> CreatePresentation(string title, string creatorId)
    {
        var presentation = new Presentation
        {
            Title = title,
            CreatedById = creatorId
        };
        _db.Presentations.Add(presentation);
        await _db.SaveChangesAsync();
        return presentation;
    }

    public async Task AddUserToPresentation(string presentationId, string userId, UserRole role)
    {
        var link = new PresentationUser
        {
            UserId = userId,
            PresentationId = presentationId,
            Role = role
        };
        _db.PresentationUsers.Add(link);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeletePresentation(string presentationId)
    {
        var presentation = await LoadFullPresentation(presentationId);
        if (presentation == null) return false;
        RemoveSlidesAndElements(presentation);
        RemoveUsers(presentation);
        _db.Presentations.Remove(presentation);
        await _db.SaveChangesAsync();
        return true;
    }

    private async Task<Presentation?> LoadFullPresentation(string id) =>
        await _db.Presentations
            .Include(p => p.Slides).ThenInclude(s => s.Elements)
            .Include(p => p.Users)
            .FirstOrDefaultAsync(p => p.Id == id);

    private void RemoveSlidesAndElements(Presentation presentation)
    {
        foreach (var slide in presentation.Slides.ToList())
        {
            _db.SlideElements.RemoveRange(slide.Elements);
            _db.Slides.Remove(slide);
        }
    }

    private void RemoveUsers(Presentation presentation)
    {
        _db.PresentationUsers.RemoveRange(presentation.Users);
    }
}
