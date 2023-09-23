using System.ComponentModel.DataAnnotations;

namespace CapitalPlacementProgram.Models
{
    public class JobItem
    {
        [Key]
        public Guid Id { get; set; }
        public JobDetails Details { get; set; }
        public ApplicationForm? ApplicationForm { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    public class JobItemDto
    {
        public Guid Id { get; set; }
        public JobDetails Details { get; set; }
        public ApplicationFormDto? ApplicationForm { get; set; }

        public JobItemDto() { }
        public JobItemDto(JobItem jobItem) 
            => (Id, Details, ApplicationForm) = (jobItem.Id, jobItem.Details, new ApplicationFormDto(jobItem.Id, jobItem.ApplicationForm));
    }
}
