namespace CapitalPlacementProgram.Models
{
    public class ApplicationForm
    {
        public CoverImage? CoverImage { get; set; }
        public PersonalInformationForm PersonalInformation { get; set; }
        public ProfileForm Profile { get; set; }
        public List<Question>? AdditionalQuestions { get; set; }
    }

    public class ApplicationFormDto
    {
        public string CoverImage { get; set; }
        public PersonalInformationForm PersonalInformation { get; set; }
        public ProfileForm Profile { get; set; }
        public List<Question>? AdditionalQuestions { get; set; }

        public ApplicationFormDto() { }
        public ApplicationFormDto(Guid id, ApplicationForm form)
            => (CoverImage, PersonalInformation, Profile, AdditionalQuestions) = ("/api/jobs/cover/"+id, form.PersonalInformation, form.Profile, form.AdditionalQuestions);
    }

    public class CoverImage
    {
        public byte[] Content { get; set; }
        public string Extension { get; set; }
    }

    public class PersonalInformationForm
    {
        public bool PhoneIsInternal { get; set; }
        public bool PhoneIsHidden { get; set; }
        public bool NationalityIsInternal { get; set; }
        public bool NationalityIsHidden { get; set; }
        public bool CurrentResidenceIsInternal { get; set; }
        public bool CurrentResidenceIsHidden { get; set; }
        public bool IdNumberIsInternal { get; set; }
        public bool IdNumberIsHidden { get; set; }
        public bool DobIsInternal { get; set; }
        public bool DobIsHidden { get; set; }
        public bool GenderIsInternal { get; set; }
        public bool GenderIsHidden { get; set; }
        public List<Question>? AdditionalQuestions { get; set; }
    }

    public class ProfileForm
    {
        public bool EducationIsMandatory { get; set; }
        public bool EducationIsHidden { get; set; }
        public bool ExperienceIsMandatory { get; set; }
        public bool ExperienceIsHidden { get; set; }
        public bool ResumeIsMandatory { get; set; }
        public bool ResumeIsHidden { get; set; }
        public List<Question>? AdditionalQuestions { get; set; }
    }
}
