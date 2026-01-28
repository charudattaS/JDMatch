using JDMatch.Application.DTOs;
using JDMatch.Domain.Entities;

namespace JDMatch.Application.Interfaces
{
    public interface IResumeMatcher
    {
        ResumeMatchResult CalculateMatch(Resume resume, JobDescription jobDescription);

    }
}
