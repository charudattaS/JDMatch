using JDMatch.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace JDMatch.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchResumeUseCase _useCase;

        public MatchController(IMatchResumeUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpPost]
        public async Task<IActionResult> Match(Guid resumeId, Guid jobDescriptionId)
        {
            var result = await _useCase.ExecuteAsync(resumeId, jobDescriptionId);
            return Ok(result);
        }
    }
}
