﻿using System.ComponentModel.DataAnnotations;

namespace OnlineMCQ.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // Dropdown: Admin, SuperAdmin, User
    }
}
