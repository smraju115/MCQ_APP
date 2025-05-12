using System.ComponentModel.DataAnnotations;

namespace OnlineMCQ.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; } // Admin, SuperAdmin, User
    }
}
