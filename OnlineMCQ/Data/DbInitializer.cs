using Microsoft.AspNetCore.Identity;
using OnlineMCQ.Models;

namespace OnlineMCQ.Data
{
    public class DbInitializer
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            //
            if (!context.Users.Any())
            {
                var passwordHasher = new PasswordHasher<AppUser>();

                var superAdmin = new AppUser
                {
                    Name = "Super Admin",
                    Email = "superadmin@gmail.com",
                    Role = "SuperAdmin"
                };
                superAdmin.PasswordHash = passwordHasher.HashPassword(superAdmin, "Superadmin@#$1234");

                var admin = new AppUser
                {
                    Name = "Admin",
                    Email = "admin@gmail.com",
                    Role = "Admin"
                };
                admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin@#$1234");

                context.Users.AddRange(superAdmin, admin);
                context.SaveChanges();
            }
        }
    }
}
