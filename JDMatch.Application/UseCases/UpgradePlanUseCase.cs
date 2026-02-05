using JDMatch.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDMatch.Application.UseCases
{
    public class UpgradePlanUseCase
    {
        private readonly IApplicationDbContext _context;

        public UpgradePlanUseCase(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(Guid userId, Guid newPlanId)
        {
            // ---------- GET USER ----------
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found");

            // ---------- GET NEW PLAN ----------
            var newPlan = await _context.Plans
                .FirstOrDefaultAsync(p => p.Id == newPlanId);

            if (newPlan == null)
                throw new Exception("Plan not found");

            // ---------- UPDATE PLAN ----------
            user.PlanId = newPlanId;

            // Reset billing cycle
            user.SubscriptionStartDate = DateTime.UtcNow;
            user.NextBillingDate = DateTime.UtcNow.AddMonths(1);

            await _context.SaveChangesAsync();
        }
    }
}
