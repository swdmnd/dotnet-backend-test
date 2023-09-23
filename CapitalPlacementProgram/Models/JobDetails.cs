namespace CapitalPlacementProgram.Models
{
    public class JobDetails
    {
        public string Title { get; set; }
        public string? Summary { get; set; }
        public string Description { get; set; }
        public List<string>? KeySkills { get; set; }
        public string? Benefits { get; set; }
        public string? Criteria { get; set; }
        public string Type { get; set; }
        public string? DateStart { get; set; }
        public string DateOpenApplication { get; set; }
        public string DateCloseApplication { get; set; }
        public long? Duration { get; set; }
        public string Location { get; set; }
        public string? MinimalQualifications { get; set; }
        public int? MaxApplicants { get; set; }
    }
}
