using JDMatch.Application.DTOs;
using JDMatch.Application.Interfaces;
using JDMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace JDMatch.Application.UseCases
{
    public class UploadResumeUseCase : IUploadResumeUseCase
    {
        private readonly IApplicationDbContext _context;
        private readonly IMatchResumeUseCase _matchUseCase;
        private readonly ITextExtractionService _textExtractor;

        public UploadResumeUseCase(
            IApplicationDbContext context,
            IMatchResumeUseCase matchUseCase,
            ITextExtractionService textExtractor)
        {
            _context = context;
            _matchUseCase = matchUseCase;
            _textExtractor = textExtractor;
        }

        public async Task<UploadResumeResponse> ExecuteAsync(
     Guid userId,
     Stream fileStream,
     string originalFileName)
        {
            // ---------- GET USER ----------
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found");

            // ---------- GET PLAN ----------
            var plan = await _context.Plans
                .FirstOrDefaultAsync(p => p.Id == user.PlanId);

            if (plan == null)
                throw new Exception("Plan not found");

            // ---------- ROLL BILLING IF NEEDED ----------
            var now = DateTime.UtcNow;

            if (now >= user.NextBillingDate)
            {
                user.SubscriptionStartDate = user.NextBillingDate;
                user.NextBillingDate = user.NextBillingDate.AddMonths(1);

                await _context.SaveChangesAsync();
            }

            // ---------- CHECK PLAN LIMIT INSIDE CURRENT BILLING WINDOW ----------
            var resumeCount = await _context.Resumes
                .CountAsync(r =>
                    r.UserId == userId &&
                    r.CreatedAt >= user.SubscriptionStartDate &&
                    r.CreatedAt < user.NextBillingDate);

            if (resumeCount >= plan.MonthlyResumeLimit)
            {
                throw new InvalidOperationException(
                    $"Upload limit reached for current billing cycle. " +
                    $"Your plan allows {plan.MonthlyResumeLimit} resumes per cycle."
                );
            }

            // ---------- SAVE FILE ----------
            var uploadsPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Uploads",
                userId.ToString());

            Directory.CreateDirectory(uploadsPath);

            var fileId = Guid.NewGuid();
            var safeFileName = $"{fileId}_{Path.GetFileName(originalFileName)}";
            var filePath = Path.Combine(uploadsPath, safeFileName);

            using (var file = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(file);
            }

            // ---------- EXTRACT TEXT ----------
            var extension = Path.GetExtension(originalFileName);
            var extractedText = await _textExtractor
                .ExtractTextAsync(filePath, extension);

            extractedText = NormalizeText(extractedText);

            // ---------- SKILL EXTRACTION ----------
            var skills = ExtractSkills(extractedText);

            // ---------- SAVE RESUME ----------
            var resume = new Resume
            {
                Id = fileId,
                UserId = userId,
                OriginalFileName = originalFileName,
                StoredFilePath = filePath,
                ExtractedText = extractedText,
                ExperienceYears = 0,
                SkillsJson = JsonSerializer.Serialize(skills),
                CreatedAt = DateTime.UtcNow
            };

            _context.Resumes.Add(resume);
            await _context.SaveChangesAsync();

            // ---------- AUTO MATCH ----------
            var response = new UploadResumeResponse
            {
                ResumeId = resume.Id
            };

            var userJds = await _context.JobDescriptions
                .Where(j => j.UserId == userId)
                .ToListAsync();

            foreach (var jd in userJds)
            {
                var result = await _matchUseCase
                    .ExecuteAsync(resume.Id, jd.Id);

                response.Matches.Add(new JobMatchSummary
                {
                    JobDescriptionId = jd.Id,
                    JobTitle = jd.Title,
                    Score = result.Score,
                    Verdict = result.Verdict
                });
            }

            // ---------- CALCULATE USAGE ----------
            var usedCount = await _context.Resumes
                .CountAsync(r =>
                    r.UserId == userId &&
                    r.CreatedAt >= user.SubscriptionStartDate &&
                    r.CreatedAt < user.NextBillingDate);

            response.Usage = new UsageInfo
            {
                Used = usedCount,
                Limit = plan.MonthlyResumeLimit,
                Remaining = plan.MonthlyResumeLimit - usedCount,
                NextBillingDate = user.NextBillingDate
            };

            return response;
        }


        // ---------------------------------------------------
        // TEXT NORMALIZATION
        // ---------------------------------------------------
        private string NormalizeText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            // Remove null characters
            text = text.Replace("\0", "");

            // Collapse multiple spaces
            text = System.Text.RegularExpressions.Regex
                .Replace(text, @"\s+", " ");

            return text.Trim();
        }

        // ---------------------------------------------------
        // IMPROVED SKILL EXTRACTION
        // ---------------------------------------------------
        private List<string> ExtractSkills(string text)
        {
            var knownSkills = new List<string>
            {
                // Backend
                "C#", ".NET", ".NET Core", "ASP.NET", "ASP.NET Core",
                "Entity Framework", "EF Core",
                "SQL", "SQL Server", "PostgreSQL",

                // Cloud
                "Azure", "Azure Functions", "Azure Service Bus",
                "AWS", "Docker", "Kubernetes",

                // Architecture
                "Microservices", "Event-Driven Architecture",
                "Clean Architecture", "CQRS",

                // Frontend
                "JavaScript", "TypeScript", "React", "Node.js",
                "Angular",

                // Databases
                "MongoDB", "Redis",

                // Other
                "REST API", "Git", "CI/CD"
            };

            var detectedSkills = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

            foreach (var skill in knownSkills)
            {
                if (text.Contains(skill, StringComparison.OrdinalIgnoreCase))
                {
                    detectedSkills.Add(skill);
                }
            }

            return detectedSkills.ToList();
        }
    }
}
