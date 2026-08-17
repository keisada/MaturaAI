using MaturaAi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MaturaAi.Data;

public class AppDbContext : IdentityDbContext<UserApplication>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Exam> Exam { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }

    public DbSet<ExamAttempt> ExamAttempts { get; set; }
    public DbSet<UserAnswer> UserAnswers { get; set; }

    public DbSet<AiHint> Hints { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

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

        modelBuilder.Entity<ExamAttempt>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<AiHint>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<UserAnswer>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne<ExamAttempt>()
                .WithMany(e => e.UserAnswers)
                .HasForeignKey(e => e.ExamAttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Question>()
                .WithMany()
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne<Answer>()
                .WithMany()
                .HasForeignKey(e => e.AnswerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<ExamAttempt>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne<Exam>()
                .WithMany()
                .HasForeignKey(e => e.ExamId);

            entity.HasOne<UserApplication>()
                .WithMany()
                .HasForeignKey(e => e.UserId);
        });

        modelBuilder.Entity<AiHint>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne<Question>().
            WithOne().
            HasForeignKey<AiHint>(e => e.QuestionId);
        });
    }
}