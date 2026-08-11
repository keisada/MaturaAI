using MaturaAi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaturaAi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamController : ControllerBase
{
    private readonly AppDbContext _context;

    public ExamController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetExams()
    {
        // Sprawdzamy czy baza odpowiada
        if (!await _context.Database.CanConnectAsync())
        {
            return StatusCode(500, "Brak połączenia z bazą Azure SQL!");
        }

        var exams = await _context.Exam.AsNoTracking().ToListAsync();
        return Ok(exams);
    }
}