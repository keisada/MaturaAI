using MaturaAi.Data;
using MaturaAi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaturaAi.Controllers;

[ApiController]
[Route("api/exams")]
public class ExamController : ControllerBase
{
    private readonly ExamService _examService; 

    public ExamController (ExamService examService)
    {
        
        _examService = examService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllExamsAsync([FromQuery]int page = 1, int pageSize = 10)
    {
        var result = await _examService.GetAllExamsAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]

    public async Task<IActionResult> GetExamByIdAsync([FromRoute] int id)
    {
        var result= await  _examService.GetExamByIdAsync(id);
        if (result == null) { return NotFound(); }
        return Ok(result);
    }

    [HttpGet("{id}/questions")]

    public async Task<IActionResult> GetQuestionsByExamIdAsync([FromRoute] int id)
    {
        return Ok(await _examService.GetQuestionsForExamAsync(id));
    }


    
}