using JDMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JDMatch.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Resume> Resumes { get; }
        DbSet<JobDescription> JobDescriptions { get; }
        DbSet<ResumeMatch> ResumeMatches { get; }
        DbSet<User> Users { get; }
        DbSet<Plan> Plans { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
