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
        // Конфигурация для PostgreSQL
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties()
                .Where(p => p.ClrType == typeof(string)))
            {
                // Устанавливаем text вместо nvarchar по умолчанию
                if (property.GetColumnType() == null)
                    property.SetColumnType("text");
            }
        }

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Nickname).IsUnique();
            entity.Property(u => u.Nickname).HasColumnType("varchar(255)");
        });

        modelBuilder.Entity<Presentation>(entity =>
        {
            entity.HasIndex(p => p.CreatedById);
            entity.Property(p => p.Title).HasColumnType("varchar(255)");
        });

        modelBuilder.Entity<PresentationUser>(entity =>
        {
            entity.HasKey(pu => new { pu.UserId, pu.PresentationId });
            
            // Явное указание типов для PostgreSQL
            entity.Property(pu => pu.Role)
                .HasConversion<string>()
                .HasColumnType("varchar(50)");
        });

        modelBuilder.Entity<Slide>(entity =>
        {
            entity.HasMany(s => s.Elements)
                .WithOne(e => e.Slide)
                .HasForeignKey(e => e.SlideId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SlideElement>(entity =>
        {
            entity.Property(e => e.Type).HasColumnType("varchar(50)");
            entity.Property(e => e.Content).HasColumnType("text");
        });
    }
}