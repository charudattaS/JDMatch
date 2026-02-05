namespace JDMatch.Application.DTOs
{
    public class CreateJobDescriptionRequest
    {
        public string Title { get; set; } = null!;
        public string Company { get; set; } = null!;
        public string DescriptionText { get; set; } = null!;
        public int? RequiredExperienceYears { get; set; }
        public List<string>? RequiredSkills { get; set; }
    }
}
