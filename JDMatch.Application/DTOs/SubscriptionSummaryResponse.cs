namespace JDMatch.Application.DTOs
{
    public class SubscriptionSummaryResponse
    {
        public string PlanName { get; set; } = null!;
        public int MonthlyLimit { get; set; }
        public int UsedThisCycle { get; set; }
        public int Remaining { get; set; }
        public DateTime SubscriptionStartDate { get; set; }
        public DateTime NextBillingDate { get; set; }
    }
}
