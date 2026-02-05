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
    public class JobDescriptionController : ControllerBase
    {
        private readonly CreateJobDescriptionUseCase _createUseCase;
        private readonly GetUserJobDescriptionsUseCase _getUserJdsUseCase;


        public JobDescriptionController(CreateJobDescriptionUseCase createUseCase, 
            GetUserJobDescriptionsUseCase getUserJdsUseCase)
        {
            _createUseCase = createUseCase;
            _getUserJdsUseCase = getUserJdsUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJobDescriptionRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim.Value);

            var jdId = await _createUseCase.ExecuteAsync(userId, request);

            return Ok(new
            {
                jobDescriptionId = jdId
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserJobDescriptions()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim.Value);

            var result = await _getUserJdsUseCase.ExecuteAsync(userId);

            return Ok(result);
        }

    }
}
