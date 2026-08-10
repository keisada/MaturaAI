using Microsoft.EntityFrameworkCore;
using MaturaAi.Models;

namespace MaturaAi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<ExamAttempt> ExamAttempts { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
    }
}