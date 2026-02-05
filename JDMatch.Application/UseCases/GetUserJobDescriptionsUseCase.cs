using JDMatch.Application.DTOs;
using JDMatch.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JDMatch.Application.UseCases
{
    public class GetUserJobDescriptionsUseCase
    {
        private readonly IApplicationDbContext _context;

        public GetUserJobDescriptionsUseCase(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<JobDescriptionSummaryResponse>> ExecuteAsync(Guid userId)
        {
            var jobDescriptions = await _context.JobDescriptions
                .Where(j => j.UserId == userId)
                .OrderByDescending(j => j.CreatedAt)
                .Select(j => new JobDescriptionSummaryResponse
                {
                    Id = j.Id,
                    Title = j.Title,
                    Company = j.Company,
                    RequiredExperienceYears = j.RequiredExperienceYears,
                    CreatedAt = j.CreatedAt
                })
                .ToListAsync();

            return jobDescriptions;
        }
    }
}
