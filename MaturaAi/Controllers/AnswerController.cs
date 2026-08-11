using MaturaAi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaturaAi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnswerController : ControllerBase
{
    private readonly AppDbContext _context;

    public AnswerController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/answer
    [HttpGet]
    public async Task<IActionResult> GetAllAnswers()
    {
        var answers = await _context.Answers.AsNoTracking().ToListAsync();
        return Ok(answers);
    }

    // GET: api/answer/question/10
    // Zwraca tylko odpowiedzi do konkretnego pytania
    [HttpGet("question/{questionId}")]
    public async Task<IActionResult> GetAnswersForQuestion(int questionId)
    {
        var answers = await _context.Answers
            .Where(a => a.QuestionId == questionId)
            .AsNoTracking()
            .ToListAsync();

        if (!answers.Any()) return NotFound("Brak odpowiedzi dla tego pytania.");

        return Ok(answers);
    }
}