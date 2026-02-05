using JDMatch.Application.DTOs;
using JDMatch.Application.Interfaces;
using JDMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace JDMatch.Application.UseCases
{
    public class CreateJobDescriptionUseCase
    {
        private readonly IApplicationDbContext _context;

        public CreateJobDescriptionUseCase(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> ExecuteAsync(Guid userId, CreateJobDescriptionRequest request)
        {
            // ---------- VALIDATE USER ----------
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found");

            if (!user.IsSubscriptionActive)
                throw new Exception("Subscription is not active");

            // ---------- CREATE JD ----------
            var jobDescription = new JobDescription
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = request.Title,
                Company = request.Company,
                DescriptionText = request.DescriptionText,
                RequiredExperienceYears = request.RequiredExperienceYears,
                RequiredSkillsJson = request.RequiredSkills != null
                    ? JsonSerializer.Serialize(request.RequiredSkills)
                    : null,
                CreatedAt = DateTime.UtcNow
            };

            _context.JobDescriptions.Add(jobDescription);
            await _context.SaveChangesAsync();

            return jobDescription.Id;
        }
    }
}
