using JDMatch.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JDMatch.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly IUploadResumeUseCase _uploadUseCase;

        public ResumeController(IUploadResumeUseCase uploadUseCase)
        {
            _uploadUseCase = uploadUseCase;
        }

        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                using var stream = file.OpenReadStream();

                var result = await _uploadUseCase.ExecuteAsync(
                    userId,
                    stream,
                    file.FileName);

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
    }
}

