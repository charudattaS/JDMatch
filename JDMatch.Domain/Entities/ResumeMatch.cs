using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDMatch.Domain.Entities
{
    public class ResumeMatch
    {
        public Guid Id { get; set; }

        public Guid ResumeId { get; set; }
        public Guid JobDescriptionId { get; set; }

        public decimal Score { get; set; }   // 0–100

        public string Verdict { get; set; } = string.Empty;
        // Example: Strong / Medium / Weak

        public string? MatchedSkillsJson { get; set; }
        public string? MissingSkillsJson { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Resume? Resume { get; set; }
        public JobDescription? JobDescription { get; set; }
    }
}
