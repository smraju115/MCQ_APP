using Microsoft.EntityFrameworkCore;
using OnlineMCQ.Models;

namespace OnlineMCQ.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserTest> UserTests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<ExamSetting> ExamSettings { get; set; }

        
    }
}
