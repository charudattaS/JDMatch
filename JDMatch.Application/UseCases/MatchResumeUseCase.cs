using JDMatch.Application.DTOs;
using JDMatch.Application.Interfaces;
using JDMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JDMatch.Application.UseCases
{
    public class MatchResumeUseCase : IMatchResumeUseCase
    {
        private readonly IApplicationDbContext _context;
        private readonly IResumeMatcher _matcher;

        public MatchResumeUseCase(IApplicationDbContext context, IResumeMatcher matcher)
        {
            _context = context;
            _matcher = matcher;
        }

        public async Task<ResumeMatchResult> ExecuteAsync(Guid resumeId, Guid jobDescriptionId)
        {
            var resume = await _context.Resumes
                .FirstOrDefaultAsync(r => r.Id == resumeId);

            var jd = await _context.JobDescriptions
                .FirstOrDefaultAsync(j => j.Id == jobDescriptionId);

            if (resume == null || jd == null)
                throw new Exception("Resume or JobDescription not found.");

            var result = _matcher.CalculateMatch(resume, jd);

            var existingMatch = await _context.ResumeMatches
                .FirstOrDefaultAsync(rm =>
                    rm.ResumeId == resumeId &&
                    rm.JobDescriptionId == jobDescriptionId);

            if (existingMatch != null)
            {
                existingMatch.Score = result.Score;
                existingMatch.Verdict = result.Verdict;
                existingMatch.MatchedSkillsJson =
                    System.Text.Json.JsonSerializer.Serialize(result.MatchedSkills);
                existingMatch.MissingSkillsJson =
                    System.Text.Json.JsonSerializer.Serialize(result.MissingSkills);
                existingMatch.CreatedAt = DateTime.UtcNow;
            }
            else
            {
                var newMatch = new ResumeMatch
                {
                    Id = Guid.NewGuid(),
                    ResumeId = resumeId,
                    JobDescriptionId = jobDescriptionId,
                    Score = result.Score,
                    Verdict = result.Verdict,
                    MatchedSkillsJson =
                        System.Text.Json.JsonSerializer.Serialize(result.MatchedSkills),
                    MissingSkillsJson =
                        System.Text.Json.JsonSerializer.Serialize(result.MissingSkills),
                    CreatedAt = DateTime.UtcNow
                };

                _context.ResumeMatches.Add(newMatch);
            }

            await _context.SaveChangesAsync();

            return result;
        }
    }
}
