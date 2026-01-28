using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDMatch.Application.DTOs
{
    public class ResumeMatchResult
    {
        public decimal Score { get; set; }
        public string Verdict { get; set; } = string.Empty;

        public List<string> MatchedSkills { get; set; } = new();
        public List<string> MissingSkills { get; set; } = new();
    }
}
