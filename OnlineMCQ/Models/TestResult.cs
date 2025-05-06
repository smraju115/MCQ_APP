using System.ComponentModel.DataAnnotations;

namespace OnlineMCQ.Models
{
    public class TestResult
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserTestId { get; set; }
        public UserTest UserTest { get; set; }

        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
    }
}
