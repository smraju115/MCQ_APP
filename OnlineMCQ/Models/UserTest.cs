using System.ComponentModel.DataAnnotations;

namespace OnlineMCQ.Models
{
    public class UserTest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Contact { get; set; } // 

        public DateTime TestDate { get; set; } = DateTime.Now;
    }
}
