using Microsoft.EntityFrameworkCore;
using OnlineMCQ.Models;
using System.Security.Claims;

namespace OnlineMCQ.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserTest> UserTests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<ExamSetting> ExamSettings { get; set; }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Notice> Notices { get; set; }

        //new field
        
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Level> Levels { get; set; }

    }
}
