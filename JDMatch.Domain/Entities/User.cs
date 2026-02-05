namespace JDMatch.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string CompanyName { get; set; } = null!;

        public Guid PlanId { get; set; }

        public Plan Plan { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
        public ICollection<Resume> Resumes { get; set; } = new List<Resume>();
        public ICollection<JobDescription> JobDescriptions { get; set; } = new List<JobDescription>();
        public string PasswordHash { get; set; } = null!;
        public DateTime SubscriptionStartDate { get; set; }
        public DateTime NextBillingDate { get; set; }
        public bool IsSubscriptionActive { get; set; }


    }
}
