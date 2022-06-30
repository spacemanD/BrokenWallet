using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {
            var flag = userManager.Users.Any();
            var users = flag ? userManager.Users.ToList() : await GetUsers(userManager);

            await context.SaveChangesAsync();
        }

        private static async Task<List<AppUser>> GetUsers(UserManager<AppUser> userManager)
        {
            var users = new List<AppUser>
            {
                new AppUser
                {
                        DisplayName = "Bob",
                        UserName = "bob",
                        Email = "bob@test.com"
                    },
                    new AppUser
                    {
                        DisplayName = "Jane",
                        UserName = "jane",
                        Email = "jane@test.com"
                    },
                    new AppUser
                    {
                        DisplayName = "Tom",
                        UserName = "tom",
                        Email = "tom@test.com"
                    },
                };

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }

            return users;
        }
    }
}