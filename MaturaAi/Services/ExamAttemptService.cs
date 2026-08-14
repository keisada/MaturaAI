using MaturaAi.Data;
using MaturaAi.DTOs;
using MaturaAi.Models;
using Microsoft.EntityFrameworkCore;

namespace MaturaAi.Services
{
    public class ExamAttemptService
    {
        private readonly AppDbContext _dbContext;

        public ExamAttemptService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int?> StartExamAttemptAsync(int examId, string userId)
        {
            if (!await _dbContext.Exam.AnyAsync(e => e.Id == examId))
            {
                return null;
            }

            var attempt = new ExamAttempt
            {
                ExamId = examId,
                UserId = userId,
                StartedAt = DateTime.UtcNow,
                Score = 0
            };

            _dbContext.ExamAttempts.Add(attempt);
            await _dbContext.SaveChangesAsync();

            return attempt.Id;
        }

        public async Task<bool> SaveAnswerAsync(int attemptId,string userId,SaveAnswerDto dto)
        {
            var attempt = await _dbContext.ExamAttempts.FirstOrDefaultAsync(e =>e.Id == attemptId &&e.UserId == userId);

            if (attempt == null)
            {
                return false;
            }

            if (attempt.EndAt != null)
            {
                return false;
            }

            var questionExists = await _dbContext.Questions.AnyAsync(q =>q.Id == dto.QuestionId &&q.ExamId == attempt.ExamId);

            if (!questionExists)
            {
                return false;
            }

            if (dto.AnswerId != null)
            {
                var answerExists = await _dbContext.Answers
                    .AnyAsync(a =>
                        a.Id == dto.AnswerId &&
                        a.QuestionId == dto.QuestionId);

                if (!answerExists)
                {
                    return false;
                }
            }

            var userAnswer = await _dbContext.UserAnswers
                .FirstOrDefaultAsync(a =>
                    a.ExamAttemptId == attemptId &&
                    a.QuestionId == dto.QuestionId);

            if (userAnswer != null)
            {
                userAnswer.AnswerId = dto.AnswerId;
                userAnswer.AnswerText = dto.AnswerText;
            }
            else
            {
                userAnswer = new UserAnswer
                {
                    ExamAttemptId = attemptId,
                    QuestionId = dto.QuestionId,
                    AnswerId = dto.AnswerId,
                    AnswerText = dto.AnswerText
                };

                _dbContext.UserAnswers.Add(userAnswer);
            }

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<int?> FinishExamAttemptAsync(int attemptId,string userId)
        {
            var attempt = await _dbContext.ExamAttempts.FirstOrDefaultAsync(e => e.Id == attemptId &&e.UserId == userId);

            if (attempt == null)
            {
                return null;
            }

            if (attempt.EndAt != null)
            {
                return null;
            }

            var userAnswers = await _dbContext.UserAnswers
                .Where(a => a.ExamAttemptId == attemptId)
                .ToListAsync();

            var answerIds = userAnswers
                .Where(a => a.AnswerId != null)
                .Select(a => a.AnswerId!.Value)
                .ToList();

            var score = await _dbContext.Answers
                .CountAsync(a =>
                    answerIds.Contains(a.Id) &&
                    a.IsCorrect);

            attempt.Score = score;
            attempt.EndAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return score;
        }
    }
}
