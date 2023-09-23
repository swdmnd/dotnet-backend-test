using System.ComponentModel.DataAnnotations;

namespace CapitalPlacementProgram.Models
{
    public class JobItem
    {
        [Key]
        public Guid Id { get; set; }
        public JobDetails Details { get; set; }
        public ApplicationForm? ApplicationForm { get; set; }
        public Workflow? Workflow { get; set; }
        public bool IsPublished { get; set; } = false;
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    public class JobItemDto
    {
        public Guid Id { get; set; }
        public JobDetails Details { get; set; }
        public ApplicationFormDto? ApplicationForm { get; set; }
        public Workflow? Workflow { get; set; }
        public bool IsPublished { get; set; } = false;

        public JobItemDto() { }
        public JobItemDto(JobItem jobItem) 
            => (Id, Details, ApplicationForm, Workflow, IsPublished) = (jobItem.Id, jobItem.Details, new ApplicationFormDto(jobItem.Id, jobItem.ApplicationForm), jobItem.Workflow, jobItem.IsPublished);
    }
}
