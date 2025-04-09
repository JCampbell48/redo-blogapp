using BloggApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BloggApp.Data
{
    public static class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context, 
                                           UserManager<ApplicationUser> userManager,
                                           RoleManager<IdentityRole> roleManager)
        {
            // Create roles if they don't exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            
            if (!await roleManager.RoleExistsAsync("Contributor"))
            {
                await roleManager.CreateAsync(new IdentityRole("Contributor"));
            }

            // Create admin user if it doesn't exist
            if (await userManager.FindByEmailAsync("a@a.a") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "a@a.a",
                    Email = "a@a.a",
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "User"
                };

                var result = await userManager.CreateAsync(adminUser, "P@$$w0rd");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Create contributor user if it doesn't exist
            if (await userManager.FindByEmailAsync("c@c.c") == null)
            {
                var contributorUser = new ApplicationUser
                {
                    UserName = "c@c.c",
                    Email = "c@c.c",
                    EmailConfirmed = true,
                    FirstName = "Contributor",
                    LastName = "User"
                };

                var result = await userManager.CreateAsync(contributorUser, "P@$$w0rd");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(contributorUser, "Contributor");
                }
            }

            // Seed sample article if no articles exist
            if (!context.Articles.Any())
            {
                context.Articles.Add(
                    new Article
                    {
                        Title = "New fires erupt in southern California",
                        Body = "Five new fires have erupted in southern California. The blazes - named Laguna, Sepulveda, Gibbel, Gilman and Border 2 - flared up on Thursday in the counties of Los Angeles, Riverside and San Diego, according to the California Department of Forestry and Fire Protection. Some evacuation orders have been issued. Officials said that hot and dry conditions across the state are increasing the risk of wildfires. The largest of the five blazes is the Border 2 Fire, which has burned more than 1,200 acres since it started on Thursday morning close to the US-Mexico border in San Diego County.",
                        CreatDate = DateTime.Now,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddMonths(1),
                        ContributorUsername = "c@c.c"
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}