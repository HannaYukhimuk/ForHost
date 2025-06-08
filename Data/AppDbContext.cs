using Microsoft.EntityFrameworkCore;
using MyPresentationApp.Models;

namespace MyPresentationApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Presentation> Presentations { get; set; }
    public DbSet<Slide> Slides { get; set; }
    public DbSet<SlideElement> SlideElements { get; set; }
    public DbSet<PresentationUser> PresentationUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Nickname)
            .IsUnique();

        modelBuilder.Entity<PresentationUser>()
            .HasKey(pu => new { pu.UserId, pu.PresentationId });

        modelBuilder.Entity<Presentation>()
            .HasIndex(p => p.CreatedById);

        modelBuilder.Entity<Slide>()
            .HasMany(s => s.Elements)
            .WithOne(e => e.Slide) 
            .HasForeignKey(e => e.SlideId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
