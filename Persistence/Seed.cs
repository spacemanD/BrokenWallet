using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public static class Seed
    {
        private static readonly Random _Random = new Random();

        private static UserManager<AppUser> _userManager;

        private static DataContext _context;

        public static async Task SeedData(DataContext context,
            UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;

            await GetCoins();
            await GetUsers();

            await _context.SaveChangesAsync();
        }

        private static async Task<List<AppUser>> GetUsers()
        {
            List<Subscription> subscriptions = await GetSubscriptions();
            List<AppUser> users;
            if (_userManager.Users.Any())
            {
                users = _userManager.Users.ToList();
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
                        Subscription = subscriptions[(_Random.Next() % (subscriptions.Count - 1))],
                        IsAdmin = true,
                        IsBanned = false
                    },
                    new AppUser
                    {
                        DisplayName = "Дмитро Прокоп’єв",
                        UserName = "honest_expert47",
                        Email = "dmytro.prokopiev@nure.ua",
                        Subscription = subscriptions[(_Random.Next() % (subscriptions.Count - 1))],
                        IsAdmin = true,
                        IsBanned = false
                    },
                    new AppUser
                    {
                        DisplayName = "Дмитро Зінченко",
                        UserName = "dimonfaraon",
                        Email = "dmytro.zinchenko1@nure.ua ",
                        Subscription = subscriptions[(_Random.Next() % (subscriptions.Count - 1))],
                        IsAdmin = false,
                        IsBanned = false
                    },
                    new AppUser
                    {
                        DisplayName = "Олександр Олійник",
                        UserName = "AlexanderOleinik",
                        Email = "oleksandr.oliinyk3@nure.ua",
                        Subscription = subscriptions[(_Random.Next() % (subscriptions.Count - 1))],
                        IsAdmin = false,
                        IsBanned = false
                    },
                    new AppUser
                    {
                        DisplayName = "Аліса Бондар",
                        UserName = "Lutierre",
                        Email = "alisa.bondar@nure.ua",
                        Subscription = subscriptions[(_Random.Next() % (subscriptions.Count - 1))],
                        IsAdmin = false,
                        IsBanned = true
                    },
                    new AppUser
                    {
                        DisplayName = "Дмитро Васильєв",
                        UserName = "udvsharp",
                        Email = "dmytro.vasyliev@nure.ua",
                        Subscription = subscriptions[(_Random.Next() % (subscriptions.Count - 1))],
                        IsAdmin = false,
                        IsBanned = true
                    }
                };


                foreach (var user in users)
                {
                    await _userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }

            return users;
        }

        private static async Task<List<Subscription>> GetSubscriptions()
        {
            List<Subscription> subscriptions;
            if (_context.Subscriptions.Any())
            {
                subscriptions = _context.Subscriptions.ToList();
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

                await _context.Subscriptions.AddRangeAsync(subscriptions);
            }

            return subscriptions;
        }

        private static async Task<List<Coin>> GetCoins()
        {
            List<Coin> coins;
            if (_context.Coins.Any())
            {
                coins = _context.Coins.ToList();
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

                await _context.Coins.AddRangeAsync(coins);
            }

            return coins;
        }
    }
}