using MaturaAi.DTOs;
using MaturaAi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaturaAi.Controllers;

[ApiController]
[Route("api/ai")]
[Authorize]
public class AiController : ControllerBase
{
    private readonly IAiService _aiService;

    public AiController(IAiService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("hint")]
    public async Task<IActionResult> GetHintAsync(
        [FromBody] AiHintRequest request)
    {
        var result = await _aiService.GetAiHintAsync(request);

        return Ok(result);
    }
}