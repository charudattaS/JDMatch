using JDMatch.Application.DTOs;
using JDMatch.Application.Interfaces;
using JDMatch.Domain.Entities;
using System.Text.Json;

namespace JDMatch.Application.Implementation
{
    public class ResumeMatcher: IResumeMatcher
    {
        public ResumeMatchResult CalculateMatch(Resume resume, JobDescription jobDescription)
        {
            var resumeSkills = DeserializeSkills(resume.SkillsJson);
            var jdSkills = DeserializeSkills(jobDescription.RequiredSkillsJson);

            var matched = resumeSkills.Intersect(jdSkills, StringComparer.OrdinalIgnoreCase).ToList();
            var missing = jdSkills.Except(resumeSkills, StringComparer.OrdinalIgnoreCase).ToList();

            decimal skillScore = jdSkills.Count == 0
                ? 0
                : (decimal)matched.Count / jdSkills.Count * 100;

            decimal experienceScore = CalculateExperienceScore(
                resume.ExperienceYears ?? 0,
                jobDescription.RequiredExperienceYears ?? 0
            );

            decimal finalScore = (skillScore * 0.7m) + (experienceScore * 0.3m);

            return new ResumeMatchResult
            {
                Score = Math.Round(finalScore, 2),
                Verdict = GetVerdict(finalScore),
                MatchedSkills = matched,
                MissingSkills = missing
            };
        }

        private List<string> DeserializeSkills(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<string>();

            return JsonSerializer.Deserialize<List<string>>(json)
                   ?? new List<string>();
        }

        private decimal CalculateExperienceScore(int resumeYears, int requiredYears)
        {
            if (requiredYears == 0)
                return 100;

            if (resumeYears >= requiredYears)
                return 100;

            return (decimal)resumeYears / requiredYears * 100;
        }

        private string GetVerdict(decimal score)
        {
            if (score >= 80) return "Strong";
            if (score >= 50) return "Medium";
            return "Weak";
        }
    }
}
