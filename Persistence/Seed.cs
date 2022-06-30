using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Seed
    {
        private static readonly Random _Random = new Random();

        private static UserManager<AppUser> _UserManager;
        
        private static DataContext _Context;

        public static async Task SeedData(DataContext context,
            UserManager<AppUser> userManager)
        {
            _UserManager = userManager;
            _Context = context;
            
            var flag = userManager.Users.Any();
            var users = flag ? userManager.Users.ToList() : await GetUsers();
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

        private static async Task<List<AppUser>> GetUsers()
        {
            List<AppUser> users;
            if (_UserManager.Users.Any())
            {
                users = _UserManager.Users.ToList();
            }
            else
            {
                users = new List<AppUser>
                {
                    new AppUser
                    {
                        DisplayName = "Андрій Долгий",
                        UserName = "Overlord",
                        Email = "andrii.dolhyi@nure.ua",
                        IsAdmin = true,
                        IsBanned = false
                    },
                    new AppUser
                    {
                        DisplayName = "Дмитро Прокоп’єв",
                        UserName = "honest_expert47",
                        Email = "dmytro.prokopiev@nure.ua",
                        IsAdmin = true,
                        IsBanned = false
                    },
                    new AppUser
                    {
                        DisplayName = "Дмитро Зінченко",
                        UserName = "dimonfaraon",
                        Email = "dmytro.zinchenko1@nure.ua ",
                        IsAdmin = false,
                        IsBanned = false
                    },
                    new AppUser
                    {
                        DisplayName = "Олександр Олійник",
                        UserName = "AlexanderOleinik",
                        Email = "oleksandr.oliinyk3@nure.ua",
                        IsAdmin = false,
                        IsBanned = false
                    },
                    new AppUser
                    {
                        DisplayName = "Аліса Бондар",
                        UserName = "Lutierre",
                        Email = "alisa.bondar@nure.ua",
                        IsAdmin = false,
                        IsBanned = true
                    },
                    new AppUser
                    {
                        DisplayName = "Дмитро Васильєв",
                        UserName = "udvsharp",
                        Email = "dmytro.vasyliev@nure.ua",
                        IsAdmin = false,
                        IsBanned = true
                    }
                };
                
                
                foreach (var user in users)
                {
                    await _UserManager.CreateAsync(user, "Pa$$w0rd");
                }
            }

            return users;
        }

        private static async Task<List<Subscription>> GetSubscriptions()
        {
            List<Subscription> subscriptions;
            if (_Context.Subscriptions.Any())
            {
                subscriptions = _Context.Subscriptions.ToList();
            }
            else
            {
                subscriptions = new List<Subscription>
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
            }

            return subscriptions;
        }

        private static async Task<List<Coin>> GetCoins()
        {
            List<Coin> coins;
            if (_Context.Coins.Any())
            {
                coins = _Context.Coins.ToList();
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
        
        private static async Task<List<Notification>> GetNotifications()
        {
            List<Notification> notifications;
            List<AppUser> users = await GetUsers(); 
            List<Coin> coins = await GetCoins(); 
            if (_Context.Notifications.Any())
            {
                notifications = _Context.Notifications.ToList();
            }
            else
            {
                notifications = new List<Notification>
                {
                    new Notification
                    {
                        Receiver = users[(_Random.Next()%(users.Count-1))],
                        Coin = coins[(_Random.Next()%(coins.Count-1))],
                        Mode = (NotificationMode)(_Random.Next()%5+1)
                    },
                    new Notification
                    {
                        Receiver = users[(_Random.Next()%(users.Count-1))],
                        Coin = coins[(_Random.Next()%(coins.Count-1))],
                        Mode = (NotificationMode)(_Random.Next()%5+1)
                    },                    
                    new Notification
                    {
                        Receiver = users[(_Random.Next()%(users.Count-1))],
                        Coin = coins[(_Random.Next()%(coins.Count-1))],
                        Mode = (NotificationMode)(_Random.Next()%5+1)
                    },
                    new Notification
                    {
                        Receiver = users[(_Random.Next()%(users.Count-1))],
                        Coin = coins[(_Random.Next()%(coins.Count-1))],
                        Mode = (NotificationMode)(_Random.Next()%5+1)
                    },
                    new Notification
                    {
                        Receiver = users[(_Random.Next()%(users.Count-1))],
                        Coin = coins[(_Random.Next()%(coins.Count-1))],
                        Mode = (NotificationMode)(_Random.Next()%5+1)
                    },
                    new Notification
                    {
                        Receiver = users[(_Random.Next()%(users.Count-1))],
                        Coin = coins[(_Random.Next()%(coins.Count-1))],
                        Mode = (NotificationMode)(_Random.Next()%5+1)
                    }
                    ,
                    new Notification
                    {
                        Receiver = users[(_Random.Next()%(users.Count-1))],
                        Coin = coins[(_Random.Next()%(coins.Count-1))],
                        Mode = (NotificationMode)(_Random.Next()%5+1)
                    }
                    ,
                    new Notification
                    {
                        Receiver = users[(_Random.Next()%(users.Count-1))],
                        Coin = coins[(_Random.Next()%(coins.Count-1))],
                        Mode = (NotificationMode)(_Random.Next()%5+1)
                    }
                    ,
                    new Notification
                    {
                        Receiver = users[(_Random.Next()%(users.Count-1))],
                        Coin = coins[(_Random.Next()%(coins.Count-1))],
                        Mode = (NotificationMode)(_Random.Next()%5+1)
                    }
                    ,
                    new Notification
                    {
                        Receiver = users[(_Random.Next()%(users.Count-1))],
                        Coin = coins[(_Random.Next()%(coins.Count-1))],
                        Mode = (NotificationMode)(_Random.Next()%5+1)
                    }
                    ,
                    new Notification
                    {
                        Receiver = users[(_Random.Next()%(users.Count-1))],
                        Coin = coins[(_Random.Next()%(coins.Count-1))],
                        Mode = (NotificationMode)(_Random.Next()%5+1)
                    }
                    ,
                    new Notification
                    {
                        Receiver = users[(_Random.Next()%(users.Count-1))],
                        Coin = coins[(_Random.Next()%(coins.Count-1))],
                        Mode = (NotificationMode)(_Random.Next()%5+1)
                    }
                    ,
                    new Notification
                    {
                        Receiver = users[(_Random.Next()%(users.Count-1))],
                        Coin = coins[(_Random.Next()%(coins.Count-1))],
                        Mode = (NotificationMode)(_Random.Next()%5+1)
                    }
                    ,
                    new Notification
                    {
                        Receiver = users[(_Random.Next()%(users.Count-1))],
                        Coin = coins[(_Random.Next()%(coins.Count-1))],
                        Mode = (NotificationMode)(_Random.Next()%5+1)
                    }
                    ,
                    new Notification
                    {
                        Receiver = users[(_Random.Next()%(users.Count-1))],
                        Coin = coins[(_Random.Next()%(coins.Count-1))],
                        Mode = (NotificationMode)(_Random.Next()%5+1)
                    }
                    ,
                    new Notification
                    {
                        Receiver = users[(_Random.Next()%(users.Count-1))],
                        Coin = coins[(_Random.Next()%(coins.Count-1))],
                        Mode = (NotificationMode)(_Random.Next()%5+1)
                    }
                    ,
                    new Notification
                    {
                        Receiver = users[(_Random.Next()%(users.Count-1))],
                        Coin = coins[(_Random.Next()%(coins.Count-1))],
                        Mode = (NotificationMode)(_Random.Next()%5+1)
                    }
                    ,
                    new Notification
                    {
                        Receiver = users[(_Random.Next()%(users.Count-1))],
                        Coin = coins[(_Random.Next()%(coins.Count-1))],
                        Mode = (NotificationMode)(_Random.Next()%5+1)
                    }
                };
            }

            return notifications;
        }
    }
}