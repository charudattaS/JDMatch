namespace JDMatch.Application.DTOs
{
    public class JobDescriptionSummaryResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Company { get; set; } = null!;
        public int? RequiredExperienceYears { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
