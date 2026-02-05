using JDMatch.Application.DTOs;
using JDMatch.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JDMatch.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubscriptionController : ControllerBase
    {
        private readonly UpgradePlanUseCase _upgradePlanUseCase;
        private readonly GetSubscriptionSummaryUseCase _summaryUseCase;

        public SubscriptionController(UpgradePlanUseCase upgradePlanUseCase,
            GetSubscriptionSummaryUseCase summaryUseCase)
        {
            _upgradePlanUseCase = upgradePlanUseCase;
            _summaryUseCase = summaryUseCase;
        }

        [HttpPost("upgrade")]
        public async Task<IActionResult> UpgradePlan([FromBody] UpgradePlanRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim.Value);

            await _upgradePlanUseCase.ExecuteAsync(userId, request.NewPlanId);

            return Ok(new
            {
                message = "Plan upgraded successfully"
            });
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSubscriptionSummary()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim.Value);

            var result = await _summaryUseCase.ExecuteAsync(userId);

            return Ok(result);
        }


    }
}
