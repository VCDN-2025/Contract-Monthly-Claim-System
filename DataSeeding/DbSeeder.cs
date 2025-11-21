using CMCS.DataSeeding;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CMCS.DataSeeding
{
    // Seeds initial roles and an HR admin user into the database
    public static class DbSeeder
    {
        // Defines the system roles
        public static class Roles
        {
            public const string HR = "HR";
            public const string AcademicManager = "AcademicManager";
            public const string ProgrammeCoordinator = "ProgrammeCoordinator";
            public const string Lecturer = "Lecturer";
        }

        // Default HR admin credentials
        private const string HR_EMAIL = "hr@cmcs.com";
        private const string HR_PASSWORD = "HRPassword123!";

        // Ensures all roles exist and creates the HR admin user if missing
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await EnsureRoleExistsAsync(roleManager, Roles.HR);
            await EnsureRoleExistsAsync(roleManager, Roles.AcademicManager);
            await EnsureRoleExistsAsync(roleManager, Roles.ProgrammeCoordinator);
            await EnsureRoleExistsAsync(roleManager, Roles.Lecturer);

            // Check if the HR user exists, create if not
            var hrUser = await userManager.FindByEmailAsync(HR_EMAIL);
            if (hrUser == null)
            {
                hrUser = new IdentityUser
                {
                    UserName = HR_EMAIL,
                    Email = HR_EMAIL,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(hrUser, HR_PASSWORD);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(hrUser, Roles.HR);
                }
            }
        }

        // Creates a role if it does not already exist
        private static async Task EnsureRoleExistsAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
