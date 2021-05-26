using ContactBookAPI.Lib.Model;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ContactBookAPI.Lib.Data
{
    public static class Seeder
    {
        public static async Task SeedData(ContactBookContext context, UserManager<AppUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();
            await SeedRole(roleManager);
            await SeedUser(userManager, context);
        }

        private static async Task SeedUser(UserManager<AppUser> userManager, ContactBookContext context)
        {
            if(userManager.FindByEmailAsync("osasfrank246@gmail.com").Result == null)
            {
                var adminUser = new AppUser
                {
                    FirstName = "Osazee",
                    LastName = "Imadonmwinyi",
                    Email = "osasfrank246@gmail.com",
                    UserName = "osasfrank246@gmail.com",
                    PhotoUrl = "theimageurl",
                    City = "Benin",
                    State = "Edo",
                    Country = "Nigeria"
                };
                IdentityResult result= userManager.CreateAsync(adminUser, "Frank@123").Result;

                if (result.Succeeded) {
                    userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                    
                }
                var user =  await userManager.FindByEmailAsync("osasfrank246@gmail.com");
                var p1 = new PhoneNumber { Number = "08177272635", AppUserId = user.Id };
                var p2 =  new PhoneNumber { Number = "09050278809", AppUserId = user.Id };
                var s1 = new Social { Name = "facebbook", Link = "myLinkToFacebook", AppUserId = user.Id };
                var s2 = new Social { Name = "Twitter", Link = "myLinkToTwitter", AppUserId = user.Id };
                await context.AddRangeAsync(p1, p2, s1, s2);
                await context.SaveChangesAsync();
            }

           
        }

        public static async Task SeedRole(RoleManager<IdentityRole> roleManager)
        {
            if (roleManager.RoleExistsAsync("Regular").Result == false)
            {
                var role = new IdentityRole
                {
                    Name = "Regular"
                };
                await roleManager.CreateAsync(role);
            }
                                               
            if(roleManager.RoleExistsAsync("Admin").Result == false)
            {
                var role = new IdentityRole
                {
                    Name = "Admin"
                };
               
                await roleManager.CreateAsync(role);             
            }
        }
    }
}
