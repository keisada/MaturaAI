using MaturaAi.Models;
using Microsoft.EntityFrameworkCore;

namespace MaturaAi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Twoje tabele w bazie
    public DbSet<Exam> Exam { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Sztywne wskazanie kluczy dla EF Core
        modelBuilder.Entity<Exam>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Question>(entity =>
        {
            entity.ToTable("Questions", "dbo");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.ToTable("Answers", "dbo");
            entity.HasKey(e => e.Id);
        });
    }
}