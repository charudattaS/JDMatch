namespace JDMatch.Domain.Entities
{
    public class JobDescription
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }   // Owner

        public string Title { get; set; } = string.Empty;

        public string Company { get; set; } = string.Empty;

        public string DescriptionText { get; set; } = string.Empty;

        public int? RequiredExperienceYears { get; set; }

        public string? RequiredSkillsJson { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public User User { get; set; }
        public ICollection<ResumeMatch> ResumeMatches { get; set; } = new List<ResumeMatch>();

    }
}
