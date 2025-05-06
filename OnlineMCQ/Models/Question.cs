using System.ComponentModel.DataAnnotations;

namespace OnlineMCQ.Models
{
    public class Question
    {
        [Key]  // Primary Key
        public int QuestionId { get; set; }

        [Required]  // এই প্রপার্টি ফাঁকা রাখা যাবে না
        [StringLength(500)]  // প্রশ্নের জন্য সর্বোচ্চ 500 ক্যারেক্টার
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
        public int CorrectAnswer { get; set; } // সঠিক উত্তর (1=A, 2=B, 3=C, 4=D)
    }
}
