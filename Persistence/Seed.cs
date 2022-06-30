using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context,
            UserManager<AppUser> userManager)
        {
            var flag = userManager.Users.Any();
            var users = flag ? userManager.Users.ToList() : await GetUsers(userManager, context);
            if (!context.Activities.Any())
            {
                var activities = new List<Activity>
                {
                    new Activity
                    {
                        Title = "Past Activity 1",
                        Date = DateTime.Now.AddMonths(-2),
                        Description = "Activity 2 months ago",
                        Category = "drinks",
                        City = "London",
                        Venue = "Pub",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[0],
                                IsHost = true
                            }
                        }
                    },
                    new Activity
                    {
                        Title = "Past Activity 2",
                        Date = DateTime.Now.AddMonths(-1),
                        Description = "Activity 1 month ago",
                        Category = "culture",
                        City = "Paris",
                        Venue = "The Louvre",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[0],
                                IsHost = true
                            },
                            new ActivityAttendee
                            {
                                AppUser = users[1],
                                IsHost = false
                            },
                        }
                    },
                    new Activity
                    {
                        Title = "Future Activity 1",
                        Date = DateTime.Now.AddMonths(1),
                        Description = "Activity 1 month in future",
                        Category = "music",
                        City = "London",
                        Venue = "Wembly Stadium",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[2],
                                IsHost = true
                            },
                            new ActivityAttendee
                            {
                                AppUser = users[1],
                                IsHost = false
                            },
                        }
                    },
                    new Activity
                    {
                        Title = "Future Activity 2",
                        Date = DateTime.Now.AddMonths(2),
                        Description = "Activity 2 months in future",
                        Category = "food",
                        City = "London",
                        Venue = "Jamies Italian",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[0],
                                IsHost = true
                            },
                            new ActivityAttendee
                            {
                                AppUser = users[2],
                                IsHost = false
                            },
                        }
                    },
                    new Activity
                    {
                        Title = "Future Activity 3",
                        Date = DateTime.Now.AddMonths(3),
                        Description = "Activity 3 months in future",
                        Category = "drinks",
                        City = "London",
                        Venue = "Pub",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[1],
                                IsHost = true
                            },
                            new ActivityAttendee
                            {
                                AppUser = users[0],
                                IsHost = false
                            },
                        }
                    },
                    new Activity
                    {
                        Title = "Future Activity 4",
                        Date = DateTime.Now.AddMonths(4),
                        Description = "Activity 4 months in future",
                        Category = "culture",
                        City = "London",
                        Venue = "British Museum",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[1],
                                IsHost = true
                            }
                        }
                    },
                    new Activity
                    {
                        Title = "Future Activity 5",
                        Date = DateTime.Now.AddMonths(5),
                        Description = "Activity 5 months in future",
                        Category = "drinks",
                        City = "London",
                        Venue = "Punch and Judy",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[0],
                                IsHost = true
                            },
                            new ActivityAttendee
                            {
                                AppUser = users[1],
                                IsHost = false
                            },
                        }
                    },
                    new Activity
                    {
                        Title = "Future Activity 6",
                        Date = DateTime.Now.AddMonths(6),
                        Description = "Activity 6 months in future",
                        Category = "music",
                        City = "London",
                        Venue = "O2 Arena",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[2],
                                IsHost = true
                            },
                            new ActivityAttendee
                            {
                                AppUser = users[1],
                                IsHost = false
                            },
                        }
                    },
                    new Activity
                    {
                        Title = "Future Activity 7",
                        Date = DateTime.Now.AddMonths(7),
                        Description = "Activity 7 months in future",
                        Category = "travel",
                        City = "Berlin",
                        Venue = "All",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[0],
                                IsHost = true
                            },
                            new ActivityAttendee
                            {
                                AppUser = users[2],
                                IsHost = false
                            },
                        }
                    },
                    new Activity
                    {
                        Title = "Future Activity 8",
                        Date = DateTime.Now.AddMonths(8),
                        Description = "Activity 8 months in future",
                        Category = "drinks",
                        City = "London",
                        Venue = "Pub",
                        Attendees = new List<ActivityAttendee>
                        {
                            new ActivityAttendee
                            {
                                AppUser = users[2],
                                IsHost = true
                            },
                            new ActivityAttendee
                            {
                                AppUser = users[1],
                                IsHost = false
                            },
                        }
                    }
                };
                
                await context.Activities.AddRangeAsync(activities);
                await context.SaveChangesAsync();
            }
        }

        private static async Task<List<AppUser>> GetUsers(UserManager<AppUser> userManager, DataContext context)
        {
            
            var users = new List<AppUser>
            {
                new AppUser
                {
                    DisplayName = "Андрій Долгий",
                    UserName = "Overlord",
                    Email = "andrii.dolhyi@nure.ua ",
                    CoinFollowings = new List<CoinFollowing>()
                    {
                        new CoinFollowing()
                        {
                        },
                    },
                    Followings = new List<UserFollowing>
                    {
                        new UserFollowing
                        {
                            
                        }
                    },
                    Followers = new List<UserFollowing>
                    {
                        new UserFollowing
                        {
                            
                        }
                    },
                    Notifications = new List<Notification>
                    {
                        new Notification
                        {
                            
                        }
                    }
                },
                new AppUser
                {
                    DisplayName = "Андрій Долгий",
                    UserName = "Overlord",
                    Email = "andrii.dolhyi@nure.ua ",
                    CoinFollowings = new List<CoinFollowing>()
                    {
                        new CoinFollowing()
                        {
                        },
                    },
                    Followings = new List<UserFollowing>
                    {
                        new UserFollowing
                        {
                            
                        }
                    },
                    Followers = new List<UserFollowing>
                    {
                        new UserFollowing
                        {
                            
                        }
                    },
                    Notifications = new List<Notification>
                    {
                        new Notification
                        {
                            
                        }
                    }
                },
                new AppUser
                {
                    DisplayName = "Андрій Долгий",
                    UserName = "Overlord",
                    Email = "andrii.dolhyi@nure.ua ",
                    CoinFollowings = new List<CoinFollowing>()
                    {
                        new CoinFollowing()
                        {
                        },
                    },
                    Followings = new List<UserFollowing>
                    {
                        new UserFollowing
                        {
                            
                        }
                    },
                    Followers = new List<UserFollowing>
                    {
                        new UserFollowing
                        {
                            
                        }
                    },
                    Notifications = new List<Notification>
                    {
                        new Notification
                        {
                            
                        }
                    }
                },
                new AppUser
                {
                    DisplayName = "Андрій Долгий",
                    UserName = "Overlord",
                    Email = "andrii.dolhyi@nure.ua ",
                    CoinFollowings = new List<CoinFollowing>()
                    {
                        new CoinFollowing()
                        {
                        },
                    },
                    Followings = new List<UserFollowing>
                    {
                        new UserFollowing
                        {
                            
                        }
                    },
                    Followers = new List<UserFollowing>
                    {
                        new UserFollowing
                        {
                            
                        }
                    },
                    Notifications = new List<Notification>
                    {
                        new Notification
                        {
                            
                        }
                    }
                },
                new AppUser
                {
                    DisplayName = "Андрій Долгий",
                    UserName = "Overlord",
                    Email = "andrii.dolhyi@nure.ua ",
                    CoinFollowings = new List<CoinFollowing>()
                    {
                        new CoinFollowing()
                        {
                        },
                    },
                    Followings = new List<UserFollowing>
                    {
                        new UserFollowing
                        {
                            
                        }
                    },
                    Followers = new List<UserFollowing>
                    {
                        new UserFollowing
                        {
                            
                        }
                    },
                    Notifications = new List<Notification>
                    {
                        new Notification
                        {
                            
                        }
                    }
                },
                new AppUser
                {
                    DisplayName = "Андрій Долгий",
                    UserName = "Overlord",
                    Email = "andrii.dolhyi@nure.ua ",
                    CoinFollowings = new List<CoinFollowing>()
                    {
                        new CoinFollowing()
                        {
                        },
                    },
                    Followings = new List<UserFollowing>
                    {
                        new UserFollowing
                        {
                            
                        }
                    },
                    Followers = new List<UserFollowing>
                    {
                        new UserFollowing
                        {
                            
                        }
                    },
                    Notifications = new List<Notification>
                    {
                        new Notification
                        {
                            
                        }
                    }
                }
            };

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }

            return users;
        }

        private static async Task<List<Subscription>> GetSubscriptions()
        {
            var subscriptions = new List<Subscription>
            {
                new Subscription
                {
                    Name = "Standard",
                    Price = 0,
                    Description = "Standard subscription for newbie",
                    Duration = TimeSpan.MaxValue
                },
                new Subscription
                {
                    Name = "Premium for month",
                    Price = 9.99m,
                    Description = "Premium subscription for month for the custom audience",
                    Duration = TimeSpan.FromDays(30)
                },
                new Subscription
                {
                    Name = "Premium for year",
                    Price = 99.99m,
                    Description = "Premium subscription for year for the regular audience",
                    Duration = TimeSpan.FromDays(365)
                }
            };

            return subscriptions;
        }

        private static async Task<List<Coin>> GetCoins(DataContext context)
        {
            List<Coin> coins;
            if (context.Coins.Any())
            {
                coins = context.Coins.ToList();
            }
            else
            {
                coins = new List<Coin>
                {
                    new Coin
                    {
                        Identifier = "",
                        DisplayName = "Dogecoin",
                        Code = "DOGE"
                    },
                    new Coin
                    {
                        Identifier = "",
                        DisplayName = "Ethereum",
                        Code = "ETH"
                    },
                    new Coin
                    {
                        Identifier = "",
                        DisplayName = "Tether",
                        Code = "USDT"
                    },
                    new Coin
                    {
                        Identifier = "",
                        DisplayName = "Cardano",
                        Code = "ADA"
                    },
                    new Coin
                    {
                        Identifier = "",
                        DisplayName = "Bitcoin",
                        Code = "BTC"
                    }
                };
            }
            

            return coins;
        }

    }
}