using MaturaAi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace MaturaAi.Data;

public class AppDbContext : IdentityDbContext<UserApplication>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Twoje tabele w bazie
    public DbSet<Exam> Exam { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }

    public DbSet <ExamAttempt > ExamAttempts { get; set; }
    public DbSet <UserAnswer> UserAnswers { get; set; }

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
        modelBuilder.Entity<Exam>()
        .HasKey(e => e.Id);

        modelBuilder.Entity<Question>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Answer>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<ExamAttempt>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<UserAnswer>()
            .HasKey(e => e.Id);

    }
}