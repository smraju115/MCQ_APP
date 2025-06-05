using System.ComponentModel.DataAnnotations;

namespace OnlineMCQ.Models
{
    public class Level
    {
        [Key]
        public int LevelId { get; set; }

        [Required]
        [StringLength(100)]
        public string LevelName { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
