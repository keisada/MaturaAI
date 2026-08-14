using MaturaAi.Data;
using MaturaAi.DTOs;
using MaturaAi.Models;

using Microsoft.EntityFrameworkCore;

namespace MaturaAi.Services
{
    public class ExamService
    {
        private readonly AppDbContext _dbContext; 

        public ExamService(AppDbContext dbContext)
        {
            _dbContext = dbContext;  
        }

        public async Task<List<ExamDto>> GetAllExamsAsync(int page, int pageSize)
        {
            var exams = await _dbContext.Exam.OrderByDescending(e=> e.ExamYear).Skip((page-1) * pageSize).Take(pageSize).ToListAsync();
            var returnedExams = exams.Select(e => new ExamDto
            {
                Title = e.Title,
                ExamMonth = e.ExamMonth,
                ExamYear = e.ExamYear,
                Subject = e.Subject,
            }).ToList();

            return returnedExams;
        }

        public async Task<ExamDto?> GetExamByIdAsync(int id)
        {
            var exam = await   _dbContext.Exam.Where(e=> e.Id == id).FirstOrDefaultAsync(); 
            if (exam == null) { return null; }
            return new ExamDto { Title = exam.Title, ExamMonth = exam.ExamMonth, ExamYear = exam.ExamYear, Subject = exam.Subject};
                
        }

        public async Task<List<QuestionDto>> GetQuestionsForExamAsync(int examId)
        {
            var questions = await _dbContext.Questions
                .Where(q => q.ExamId == examId)
                .ToListAsync();

            var questionsIds = questions
                .Select(q => q.Id)
                .ToList();

            var answers = await _dbContext.Answers
                .Where(a => questionsIds.Contains(a.QuestionId))
                .Select(a => new 
                {
                    a.Id,
                    a.QuestionId,
                    a.Content
                })
                .ToListAsync();

            var returnedQuestions = questions
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    TaskNumber = q.TaskNumber,
                    QuestionType = q.QuestionType,
                    Content = q.Content,

                    Answers = answers
                        .Where(a => a.QuestionId == q.Id)
                        .Select(a => new AnswerDto
                        {
                            Id = a.Id,
                            Content = a.Content
                        })
                        .ToList()
                })
                .ToList();

            return returnedQuestions;
        }

    }
}
