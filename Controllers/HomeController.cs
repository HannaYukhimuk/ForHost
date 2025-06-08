using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPresentationApp.Data;
using MyPresentationApp.Models;
using MyPresentationApp.Services;

namespace MyPresentationApp.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    private readonly PresentationService _presentationService;

    public HomeController(AppDbContext db, PresentationService presentationService)
    {
        _db = db;
        _presentationService = presentationService;
    }

    public IActionResult Index() => View(GetAllPresentations());

    private List<Presentation> GetAllPresentations() =>
        _db.Presentations.Include(p => p.CreatedBy).ToList();

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(Presentation model, string nickname)
    {
        if (!ModelState.IsValid) return View(model);

        var user = CreateUser(nickname);
        SavePresentation(model, user);

        return RedirectToAction("Presentation", new { id = model.Id });
    }

    private User CreateUser(string nickname)
    {
        var user = new User { Nickname = nickname };
        _db.Users.Add(user);
        return user;
    }

    private void SavePresentation(Presentation model, User user)
    {
        model.CreatedBy = user;
        _db.Presentations.Add(model);
        _db.SaveChanges();
    }

    public IActionResult Presentation(string id, string nickname)
    {
        var presentation = LoadFullPresentation(id);
        if (presentation == null) return NotFound();

        var user = GetOrCreateUser(nickname);
        var role = AssignUserToPresentation(presentation, user);

        SetViewBag(user, role, presentation);
        SetFirstSlide(presentation);

        return View(presentation);
    }

    private Presentation? LoadFullPresentation(string id) =>
        _db.Presentations
            .Include(p => p.Slides).ThenInclude(s => s.Elements)
            .Include(p => p.Users).ThenInclude(pu => pu.User)
            .Include(p => p.CreatedBy)
            .FirstOrDefault(p => p.Id == id);

    private User GetOrCreateUser(string nickname)
    {
        var user = _db.Users.FirstOrDefault(u => u.Nickname == nickname);
        if (user != null) return user;

        user = new User { Nickname = nickname };
        _db.Users.Add(user);
        _db.SaveChanges();
        return user;
    }

    private UserRole AssignUserToPresentation(Presentation presentation, User user)
    {
        var existing = presentation.Users.FirstOrDefault(pu => pu.UserId == user.Id);
        if (existing != null) return existing.Role;

        var role = (user.Nickname == presentation.CreatedBy.Nickname)
            ? UserRole.Creator
            : UserRole.Viewer;

        var presentationUser = new PresentationUser
        {
            UserId = user.Id,
            PresentationId = presentation.Id,
            Role = role
        };

        _db.PresentationUsers.Add(presentationUser);
        _db.SaveChanges();
        return role;
    }

    private void SetViewBag(User user, UserRole role, Presentation presentation)
    {
        ViewBag.UserRole = role;
        ViewBag.IsCreator = role == UserRole.Creator;
        ViewBag.IsEditor = role is UserRole.Editor or UserRole.Creator;
        ViewBag.CurrentUser = user;
    }

    private void SetFirstSlide(Presentation presentation)
    {
        if (!presentation.Slides.Any()) return;
        ViewBag.CurrentSlideId = presentation.Slides.OrderBy(s => s.Order).First().Id;
    }

    [HttpPost]
    public IActionResult Delete(string id, string nickname)
    {
        var presentation = LoadFullPresentation(id);
        if (presentation == null) return NotFound();

        if (!IsUserCreator(presentation, nickname))
            return ShowDeleteError();

        DeletePresentationWithRelations(presentation);
        return RedirectToAction("Index");
    }

    private bool IsUserCreator(Presentation presentation, string nickname) =>
        nickname == presentation.CreatedBy.Nickname;

    private IActionResult ShowDeleteError()
    {
        TempData["ErrorMessage"] = "Insufficient permissions - only the creator can delete this presentation";
        return RedirectToAction("Index");
    }

    private void DeletePresentationWithRelations(Presentation presentation)
    {
        foreach (var slide in presentation.Slides.ToList())
        {
            _db.SlideElements.RemoveRange(slide.Elements);
            _db.Slides.Remove(slide);
        }

        _db.PresentationUsers.RemoveRange(presentation.Users);
        _db.Presentations.Remove(presentation);
        _db.SaveChanges();
    }
}
