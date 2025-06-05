using System.ComponentModel.DataAnnotations;

namespace OnlineMCQ.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }

        [Required]
        [StringLength(100)]
        public string SubjectName { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
