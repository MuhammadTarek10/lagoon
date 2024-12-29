using Lagoon.Application.Common.Interfaces;
using Lagoon.Application.Utilities;
using Lagoon.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lagoon.Infrastructure.Data
{
    public class DbInitializer : IDbInitializer
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public DbInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }

                if (!_roleManager.RoleExistsAsync(SD.AdminEndUser).GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole(SD.AdminEndUser));
                    await _roleManager.CreateAsync(new IdentityRole(SD.CustomerEndUser));
                    await _userManager.CreateAsync(new ApplicationUser
                    {
                        UserName = "mohammadtarek000@gmail.com",
                        Email = "mohammadtarek000@gmail.com",
                        Name = "Muhammad Tarek",
                        NormalizedUserName = "ADMIN@TAREK.COM",
                        NormalizedEmail = "ADMIN@TAREK.COM",
                        PhoneNumber = "1112223333",
                    }, "Admin123*");

                    ApplicationUser? user = _context.Users.FirstOrDefault(u => u.Email == "mohammadtarek000@gmail.com");
                    await _userManager.AddToRoleAsync(user!, SD.AdminEndUser);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
