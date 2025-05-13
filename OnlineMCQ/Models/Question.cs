using System.ComponentModel.DataAnnotations;

namespace OnlineMCQ.Models
{
    public class Question
    {
        [Key]  // Primary Key
        public int QuestionId { get; set; }

        [Required]  // 
        [StringLength(500)]  //
        public string QuestionText { get; set; }

        [Required]
        [StringLength(200)]
        public string OptionA { get; set; }

        [Required]
        [StringLength(200)]
        public string OptionB { get; set; }

        [Required]
        [StringLength(200)]
        public string OptionC { get; set; }

        [Required]
        [StringLength(200)]
        public string OptionD { get; set; }

        [Required]
        public int CorrectAnswer { get; set; } // Correct Answer (1=A, 2=B, 3=C, 4=D)
    }
}
