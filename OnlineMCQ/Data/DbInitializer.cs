using Microsoft.AspNetCore.Identity;
using OnlineMCQ.Models;

namespace OnlineMCQ.Data
{
    public class DbInitializer
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // যদি কোন ইউজার না থাকে তাহলে SuperAdmin & Admin ইউজার ইনসার্ট করো
            if (!context.Users.Any())
            {
                var passwordHasher = new PasswordHasher<AppUser>();

                var superAdmin = new AppUser
                {
                    Name = "Super Admin",
                    Email = "superadmin@example.com",
                    Role = "SuperAdmin"
                };
                superAdmin.PasswordHash = passwordHasher.HashPassword(superAdmin, "123456");

                var admin = new AppUser
                {
                    Name = "Admin",
                    Email = "admin@example.com",
                    Role = "Admin"
                };
                admin.PasswordHash = passwordHasher.HashPassword(admin, "123456");

                context.Users.AddRange(superAdmin, admin);
                context.SaveChanges();
            }
        }
    }
}
