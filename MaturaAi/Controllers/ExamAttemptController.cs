using MaturaAi.DTOs;
using MaturaAi.Models;
using MaturaAi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MaturaAi.Controllers
{
    [ApiController]
    [Route("api/exam-attempts")]
    [Authorize]
    public class ExamAttemptController : ControllerBase
    {
        private readonly ExamAttemptService _examAttemptService;
        private readonly UserManager<UserApplication> _userManager;

        public ExamAttemptController(ExamAttemptService examAttemptService,UserManager<UserApplication> userManager)
        {
            _examAttemptService = examAttemptService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> StartExamAttemptAsync([FromBody] int examId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var attemptId = await _examAttemptService.StartExamAttemptAsync(examId, user.Id);

            if (attemptId == null)
            {
                return NotFound("Exam not found");
            }

            return Ok(new
            {
                attemptId = attemptId.Value
            });
        }

        [HttpPost("{attemptId}/answers")]
        public async Task<IActionResult> SaveAnswerAsync(
            [FromRoute] int attemptId,
            [FromBody] SaveAnswerDto dto)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _examAttemptService
                .SaveAnswerAsync(
                    attemptId,
                    user.Id,
                    dto);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("{attemptId}/finish")]
        public async Task<IActionResult> FinishExamAttemptAsync(
            [FromRoute] int attemptId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var score = await _examAttemptService
                .FinishExamAttemptAsync(
                    attemptId,
                    user.Id);

            if (score == null)
            {
                return BadRequest();
            }

            return Ok(new
            {
                score = score.Value
            });
        }
    }
}