namespace CapitalPlacementProgram.Models
{
    public class Question
    {
        public string Type { get; set; }
        public string QuestionText { get; set; }
        public List<string>? Choices { get; set; }
        public int? MaxChoiceAllowed { get; set; }
    }

    public record QuestionType
    {
        static string Paragraph = "paragraph";
        static string ShortAnswer = "shortAnswer";
        static string YesNo = "yesNo";
        static string Dropdown = "dropdown";
        static string MultipleChoice = "multipleChoice";
        static string Date = "date";
        static string Number = "number";
        static string FileUpload = "file";
        static string VideoQuestion = "video";

        static List<string> Types = new List<string> {
            Paragraph, ShortAnswer, YesNo, Dropdown, MultipleChoice, Date, Number, FileUpload, VideoQuestion
        };

    }
}
