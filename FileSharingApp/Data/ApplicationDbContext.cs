using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FileSharingApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>  // its inherit from IdentityDbContext becuz we need users table in our databse
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<Uploads> Uploads { get; set; }
        public DbSet<Contact> Contact { get; set; }
        //public DbSet<AppUser> AppUser { get; set; }

    }
}
