using System.ComponentModel.DataAnnotations;

namespace OnlineMCQ.Models
{
    public class Notice
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string FileName { get; set; }=string.Empty;

        public string FilePath { get; set; }= string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
