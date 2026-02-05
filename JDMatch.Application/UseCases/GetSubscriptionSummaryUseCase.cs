using JDMatch.Application.DTOs;
using JDMatch.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JDMatch.Application.UseCases
{
    public class GetSubscriptionSummaryUseCase
    {
        private readonly IApplicationDbContext _context;

        public GetSubscriptionSummaryUseCase(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SubscriptionSummaryResponse> ExecuteAsync(Guid userId)
        {
            // ---------- GET USER ----------
            var user = await _context.Users
                .Include(u => u.Plan)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found");

            if (!user.IsSubscriptionActive)
                throw new Exception("Subscription is not active");

            var now = DateTime.UtcNow;

            // ---------- COUNT RESUMES IN CURRENT CYCLE ----------
            var usedThisCycle = await _context.Resumes
                .Where(r =>
                    r.UserId == userId &&
                    r.CreatedAt >= user.SubscriptionStartDate &&
                    r.CreatedAt < user.NextBillingDate)
                .CountAsync();

            var remaining = user.Plan.MonthlyResumeLimit - usedThisCycle;
            if (remaining < 0) remaining = 0;

            return new SubscriptionSummaryResponse
            {
                PlanName = user.Plan.Name,
                MonthlyLimit = user.Plan.MonthlyResumeLimit,
                UsedThisCycle = usedThisCycle,
                Remaining = remaining,
                SubscriptionStartDate = user.SubscriptionStartDate,
                NextBillingDate = user.NextBillingDate
            };
        }
    }
}
