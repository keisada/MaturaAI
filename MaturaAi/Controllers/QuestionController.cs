using MaturaAi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaturaAi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionController : ControllerBase
{
    private readonly AppDbContext _context;

    public QuestionController(AppDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<IActionResult> GetAllQuestions()
    {
        var questions = await _context.Questions.AsNoTracking().ToListAsync();
        return Ok(questions);
    }


    [HttpGet("exam/{examId}")]
    public async Task<IActionResult> GetQuestionsByExam(int examId)
    {
        var questions = await _context.Questions
            .Where(q => q.ExamId == examId)
            .AsNoTracking()
            .ToListAsync();

        if (!questions.Any()) return NotFound("Brak pytań dla tego egzaminu.");

        return Ok(questions);
    }
}