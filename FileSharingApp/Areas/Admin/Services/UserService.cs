using FileSharingApp.Areas.Admin.Models;
using FileSharingApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FileSharingApp.Areas.Admin.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }



        public async Task<OperationResult> BlockUser(string userId)
        {
            var existedUser = await context.Users.FindAsync(userId);
            if (existedUser == null)
            {
                return OperationResult.NotFound();
            }

            // this will work like a toggle (true or false)
            existedUser.IsBlocked = !existedUser.IsBlocked;
            context.Update(existedUser);
            await context.SaveChangesAsync();

            return OperationResult.Succeeded();
        }




        public IQueryable<AdminUserViewModel> GetAll()
        {
            var result = context.Users.Select(u => new AdminUserViewModel
            {
                Id= u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                IsBlocked = u.IsBlocked,
                CreatedDate = u.CreatedDate,
            });

            return result;
        }

        public IQueryable<AdminUserViewModel> GetBlockedUsers()
        {
            var result = context.Users.Where(u => u.IsBlocked).Select(u => new AdminUserViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                IsBlocked = u.IsBlocked,
                CreatedDate = u.CreatedDate,
            });

            return result;
        }


        public IQueryable<AdminUserViewModel> Search(string term)
        {
            var result = context.Users.Where(u => u.Email == term 
            || u.FirstName.Contains(term) || u.LastName.Contains(term)
            || (u.FirstName + " " + u.LastName).Contains(term) || u.Email.Contains(term))
                .Select(u => new AdminUserViewModel
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    IsBlocked = u.IsBlocked,
                    CreatedDate = u.CreatedDate,
                });

            return result;
        }

        public async Task<int> UserRegistrationCount()
        {
            var count = await context.Users.CountAsync();
            return count;
        }

        public async Task<int> UserRegistrationCount(int month)
        {
            var year = DateTime.Today.Year;

            var count = await context.Users.CountAsync(u => u.CreatedDate.Month == month && u.CreatedDate.Year == year);
            return count;
        }




        public async Task Initialize()
        {
            // check if admin role is exist if not create one 
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }

            // check if admin is exist if not create one 
            if (await userManager.FindByEmailAsync("admin@admin.com") == null)
            {
                var admin = new ApplicationUser
                {
                    FirstName = "Admin",
                    Email = "admin@admin.com",
                    UserName = "admin@admin.com"
                };

                await userManager.CreateAsync(admin, "Admin@123");

                await userManager.AddToRoleAsync(admin, UserRoles.Admin);
            }
        }
    }
}
