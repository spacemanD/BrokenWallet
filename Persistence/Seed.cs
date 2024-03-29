using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public static class Seed
    {
        private static Random _random;
        private static UserManager<AppUser> _userManager;
        private static DataContext _context;

        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {
            _random = new Random();
            _userManager = userManager;
            _context = context;

            await _context.SaveChangesAsync();

            await GetCoins();
            await GetUsers();

            await _context.SaveChangesAsync();
        }

        private static async Task GetUsers()
        {
            var subscriptions = await GetSubscriptions();

            if (!_userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new()
                    {
                        DisplayName = "Andrii Dolhyi",
                        UserName = "Overlord",
                        Email = "andrii.dolhyi@nure.ua",
                        Subscription = subscriptions[_random.Next() % (subscriptions.Count - 1)],
                        IsAdmin = true,
                        IsBanned = false
                    },
                    new()
                    {
                        DisplayName = "Dmytro Prokopiev",
                        UserName = "honest_expert48",
                        Email = "dmytro.prokopiev@nure.ua",
                        Subscription = subscriptions[_random.Next() % (subscriptions.Count - 1)],
                        IsAdmin = true,
                        IsBanned = false
                    },
                    new()
                    {
                        DisplayName = "Dmytro Zinchenko",
                        UserName = "dimonfaraon",
                        Email = "dmytro.zinchenko1@nure.ua",
                        Subscription = subscriptions[_random.Next() % (subscriptions.Count - 1)],
                        IsAdmin = false,
                        IsBanned = false
                    },
                    new()
                    {
                        DisplayName = "Oleksandr Oliinyk",
                        UserName = "AlexanderOleinik",
                        Email = "oleksandr.oliinyk3@nure.ua",
                        Subscription = subscriptions[_random.Next() % (subscriptions.Count - 1)],
                        IsAdmin = false,
                        IsBanned = false
                    },
                    new()
                    {
                        DisplayName = "Alisa Bondar",
                        UserName = "Lutierre",
                        Email = "alisa.bondar@nure.ua",
                        Subscription = subscriptions[_random.Next() % (subscriptions.Count - 1)],
                        IsAdmin = false,
                        IsBanned = true
                    },
                    new()
                    {
                        DisplayName = "Dmytro Vasyliev",
                        UserName = "udvsharp",
                        Email = "dmytro.vasyliev@nure.ua",
                        Subscription = subscriptions[_random.Next() % (subscriptions.Count - 1)],
                        IsAdmin = false,
                        IsBanned = true
                    }
                };

                foreach (var user in users)
                {
                    await _userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }
        }

        private static async Task<List<Subscription>> GetSubscriptions()
        {
            List<Subscription> subscriptions;

            if (_context.Subscriptions.Any())
            {
                subscriptions = await _context.Subscriptions.ToListAsync();
            }
            else
            {
                subscriptions = new List<Subscription>
                {
                    new()
                    {
                        Name = "Standard",
                        Price = 0,
                        Description = "Standard subscription for newbie",
                        Duration = TimeSpan.MaxValue
                    },
                    new()
                    {
                        Name = "Premium for months",
                        Price = 9.99m,
                        Description = "Premium subscription for month for the custom audience",
                        Duration = TimeSpan.FromDays(30)
                    },
                    new()
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

        private static async Task GetCoins()
        {
            if (!_context.Coins.Any())
            {
                var coins = new List<Coin>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Identifier = "dogecoin",
                        DisplayName = "Dogecoin",
                        Code = "DOGE"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Identifier = "ethereum",
                        DisplayName = "Ethereum",
                        Code = "ETH"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Identifier = "tether",
                        DisplayName = "Tether",
                        Code = "USDT"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Identifier = "cardano",
                        DisplayName = "Cardano",
                        Code = "ADA"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Identifier = "bitcoin",
                        DisplayName = "Bitcoin",
                        Code = "BTC"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Identifier = "usd-coin",
                        DisplayName = "USD Coin",
                        Code = "USDC"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Identifier = "polkadot",
                        DisplayName = "Polkadot",
                        Code = "DOT"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Identifier = "binancecoin",
                        DisplayName = "Binancecoin",
                        Code = "BNB"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Identifier = "solana",
                        DisplayName = "Solana",
                        Code = "SOL"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Identifier = "ripple",
                        DisplayName = "Ripple",
                        Code = "XRP"
                    }
                };

                await _context.Coins.AddRangeAsync(coins);
            }
        }
    }
}