namespace JDMatch.Application.DTOs
{
    public class UploadResumeResponse
    {
        public Guid ResumeId { get; set; }
        public List<JobMatchSummary> Matches { get; set; } = new();
    }

    public class JobMatchSummary
    {
        public Guid JobDescriptionId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public decimal Score { get; set; }
        public string Verdict { get; set; } = string.Empty;
    }

}
