using Microsoft.AspNetCore.Mvc;
using MyPresentationApp.Services;
using MyPresentationApp.Models.DTOs;
using MyPresentationApp.Data;
using MyPresentationApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MyPresentationApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PresentationsController : ControllerBase
{
    private readonly PresentationService _presentationService;
    private readonly AppDbContext _db;

    public PresentationsController(
        PresentationService presentationService,
        AppDbContext db)
    {
        _presentationService = presentationService;
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePresentationRequest request)
    {
        var presentation = await CreatePresentationAsync(request);
        return Ok(presentation);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, [FromQuery] string nickname)
    {
        var result = await TryDeletePresentationAsync(id, nickname);
        return result ? NoContent() : NotFound();
    }

    [HttpPost("{presentationId}/join")]
    public async Task<IActionResult> JoinPresentation(
        string presentationId,
        [FromBody] JoinPresentationRequest request)
    {
        var (success, response) = await TryJoinPresentationAsync(presentationId, request);
        return success ? Ok(response) : NotFound("Presentation not found");
    }

    [HttpPost("{presentationId}/promote/{userId}")]
    public async Task<IActionResult> PromoteUser(string presentationId, string userId)
    {
        var result = await TryPromoteUserAsync(presentationId, userId);
        return result ? Ok(new { Message = "User promoted to editor" }) 
                      : NotFound("User not found in this presentation");
    }

    private async Task<Presentation> CreatePresentationAsync(CreatePresentationRequest request)
    {
        var presentation = await _presentationService.CreatePresentation(request.Title, request.CreatorId);
        await _presentationService.AddUserToPresentation(presentation.Id, request.CreatorId, UserRole.Creator);
        return presentation;
    }

    private async Task<bool> TryDeletePresentationAsync(string id, string nickname)
    {
        var presentation = await GetPresentationWithCreatorAsync(id);
        if (presentation == null || !IsCreator(presentation, nickname))
            return false;

        return await _presentationService.DeletePresentation(id);
    }

    private async Task<(bool success, object response)> TryJoinPresentationAsync(
        string presentationId, 
        JoinPresentationRequest request)
    {
        var presentation = await GetPresentationWithUsersAsync(presentationId);
        if (presentation == null) return (false, null);

        var user = await GetOrCreateUserAsync(request.Nickname);
        var existingRole = GetExistingUserRole(presentation, user.Id);
        if (existingRole != null) return (true, CreateUserResponse(existingRole.Value, user.Id));

        var role = DetermineUserRole(presentation, user);
        await AddUserToPresentationAsync(presentationId, user.Id, role);

        return (true, CreateUserResponse(role, user.Id));
    }

    private async Task<bool> TryPromoteUserAsync(string presentationId, string userId)
    {
        var presentationUser = await GetPresentationUserAsync(presentationId, userId);
        if (presentationUser == null) return false;

        presentationUser.Role = UserRole.Editor;
        await _db.SaveChangesAsync();
        return true;
    }

    private async Task<Presentation> GetPresentationWithCreatorAsync(string id) =>
        await _db.Presentations
            .Include(p => p.CreatedBy)
            .FirstOrDefaultAsync(p => p.Id == id);

    private async Task<Presentation> GetPresentationWithUsersAsync(string id) =>
        await _db.Presentations
            .Include(p => p.CreatedBy)
            .Include(p => p.Users)
                .ThenInclude(pu => pu.User)
            .FirstOrDefaultAsync(p => p.Id == id);

    private bool IsCreator(Presentation presentation, string nickname) =>
        presentation.CreatedBy?.Nickname == nickname;

    private async Task<User> GetOrCreateUserAsync(string nickname)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Nickname == nickname);
        if (user != null) return user;

        user = new User { Nickname = nickname };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    private UserRole? GetExistingUserRole(Presentation presentation, string userId) =>
        presentation.Users
            .FirstOrDefault(pu => pu.UserId == userId)?
            .Role;

    private UserRole DetermineUserRole(Presentation presentation, User user) =>
        user.Nickname == presentation.CreatedBy?.Nickname 
            ? UserRole.Creator 
            : UserRole.Viewer;

    private async Task AddUserToPresentationAsync(string presentationId, string userId, UserRole role)
    {
        _db.PresentationUsers.Add(new PresentationUser
        {
            UserId = userId,
            PresentationId = presentationId,
            Role = role
        });
        await _db.SaveChangesAsync();
    }

    private object CreateUserResponse(UserRole role, string userId) =>
        new { Role = role.ToString(), UserId = userId };

    private async Task<PresentationUser> GetPresentationUserAsync(string presentationId, string userId) =>
        await _db.PresentationUsers
            .FirstOrDefaultAsync(pu => pu.PresentationId == presentationId && pu.UserId == userId);
}