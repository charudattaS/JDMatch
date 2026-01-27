namespace JDMatch.Domain.Entities
{
    public class Plan
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal MonthlyPrice { get; set; }

        public int MonthlyResumeLimit { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
