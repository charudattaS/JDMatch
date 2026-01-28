using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDMatch.Domain.Entities
{
    public class Resume
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        // Original uploaded file info
        public string OriginalFileName { get; set; } = string.Empty;
        public string StoredFilePath { get; set; } = string.Empty;

        // Extracted content (very important for AI & matching)
        public string ExtractedText { get; set; } = string.Empty;

        // Metadata
        public int? ExperienceYears { get; set; }
        public string? SkillsJson { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public User? User { get; set; }
    }
}
