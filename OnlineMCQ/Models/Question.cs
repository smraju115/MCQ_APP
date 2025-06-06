﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

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

        // New Fields for module and level

        [Required]
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }

        [Required]
        public int LevelId { get; set; }
        public Level? Level { get; set; }

    }
}
