using JDMatch.Application.Implementation;
using JDMatch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDMatch.Tests
{
    public class ResumeMatcherTests
    {
        private readonly ResumeMatcher _matcher = new();

        [Fact]
        public void CalculateMatch_Should_Return_Strong_When_Skills_And_Experience_Match()
        {
            var resume = new Resume
            {
                SkillsJson = "[\"C#\", \"SQL\", \"Azure\"]",
                ExperienceYears = 5
            };

            var jd = new JobDescription
            {
                RequiredSkillsJson = "[\"C#\", \"SQL\"]",
                RequiredExperienceYears = 3
            };

            var result = _matcher.CalculateMatch(resume, jd);

            Assert.True(result.Score >= 80);
            Assert.Equal("Strong", result.Verdict);
        }
    }
}
