namespace MyPresentationApp.Models;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); 
    public string? Nickname { get; set; }                       
    public string? SocketId { get; set; }                      
    public string? CurrentPresentationId { get; set; }         
    public ICollection<PresentationUser>? Presentations { get; set; } 
}
