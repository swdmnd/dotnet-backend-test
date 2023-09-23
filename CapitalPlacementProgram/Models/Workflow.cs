namespace CapitalPlacementProgram.Models
{
    public class Workflow
    {
        public List<ApplicationStage> Stages { get; set; }
        public bool HideFromApplicants { get; set; } = false;
    }
    public class ApplicationStage
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string? QuestionText { get; set; }
        public string? AdditionalInformation { get; set; }
        public long? Duration { get; set; }
        public string? DurationUnit { get; set; }
        public int? SubmissionDeadlineInDays { get; set; }
    }
}
