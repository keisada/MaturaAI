using Microsoft.AspNetCore.Mvc;
using MaturaAi.DTOs;
using MaturaAi.Services;


namespace MaturaAi.Controllers
{
    [ApiController]
    [Route("api/ai")]
    public class AiController :ControllerBase
    {
        private readonly IAiService _aiService; 

        public AiController(IAiService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("hint")]
        public async Task<ActionResult<AiHintResponse>> GetHint(AiHintRequest request)
        {
            var response = await _aiService.GetAiHintAsync(request);
            return Ok(response);
        }
    }
}
