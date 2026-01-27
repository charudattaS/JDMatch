using JDMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JDMatch.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var starterPlanId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var proPlanId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var agencyPlanId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            modelBuilder.Entity<Plan>().HasData(
                new Plan
                {
                    Id = starterPlanId,
                    Name = "Starter",
                    MonthlyPrice = 999,
                    MonthlyResumeLimit = 50,
                    CreatedAt = DateTime.UtcNow
                },
                new Plan
                {
                    Id = proPlanId,
                    Name = "Pro",
                    MonthlyPrice = 2999,
                    MonthlyResumeLimit = 300,
                    CreatedAt = DateTime.UtcNow
                },
                new Plan
                {
                    Id = agencyPlanId,
                    Name = "Agency",
                    MonthlyPrice = 4999,
                    MonthlyResumeLimit = 1000,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }


        public DbSet<User> Users => Set<User>();
        public DbSet<Plan> Plans => Set<Plan>();
    }
}
