namespace TheEventApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TheEventApp.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TheEventApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(TheEventApp.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            if (context.Roles.Where(p => p.Name == "Admin").FirstOrDefault() == null)
                context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole("Admin"));
            if (context.Roles.Where(p => p.Name == "Eventor").FirstOrDefault() == null)
                context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole("Eventor"));

            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            if (context.Users.Where(p => p.Email == "mcevoy4@gmail.com").FirstOrDefault() == null)
            {

                ApplicationUser user = new ApplicationUser
                {
                    UserName = "mcevoy4@gmail.com",
                    Email = "mcevoy4@gmail.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "Admin@123").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user.Id, "Admin").Wait();
                    userManager.AddToRoleAsync(user.Id, "Eventor").Wait();
                }
            }
        }
    }
}
