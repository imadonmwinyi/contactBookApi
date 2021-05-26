using ContactBookAPI.Lib.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactBookAPI.Lib.Data
{
    public class ContactBookContext:IdentityDbContext
    {
        public ContactBookContext(DbContextOptions<ContactBookContext> options):base(options)
        {

        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Social> Socials { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }

    }
}
