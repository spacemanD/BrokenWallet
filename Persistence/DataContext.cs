using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DbSet<Coin> Coins { get; set; }

        public DbSet<CoinFollowing> CoinFollowings { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<UserFollowing> UserFollowings { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CoinFollowing>(following =>
                following.HasKey(options => new
                {
                    options.AppUserId, options.CoinId
                }));

            builder.Entity<CoinFollowing>()
                .HasOne(following => following.AppUser)
                .WithMany(user => user.CoinFollowings)
                .HasForeignKey(following => following.AppUserId);

            builder.Entity<CoinFollowing>()
                .HasOne(following => following.Coin)
                .WithMany(coin => coin.Followers)
                .HasForeignKey(following => following.CoinId);

            builder.Entity<Comment>()
                .HasOne(comment => comment.Coin)
                .WithMany(coin => coin.Comments)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserFollowing>(following =>
            {
                following.HasKey(userFollowing => new
                {
                    userFollowing.ObserverId, userFollowing.TargetId
                });

                following.HasOne(userFollowing => userFollowing.Observer)
                    .WithMany(user => user.Followings)
                    .HasForeignKey(userFollowing => userFollowing.ObserverId)
                    .OnDelete(DeleteBehavior.Cascade);

                following.HasOne(userFollowing => userFollowing.Target)
                    .WithMany(user => user.Followers)
                    .HasForeignKey(userFollowing => userFollowing.TargetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Notification>(options =>
                options.HasKey(notification => new
                {
                    notification.ReceiverId, notification.CoinId
                }));

            builder.Entity<Notification>()
                .HasOne(notification => notification.Receiver)
                .WithMany(user => user.Notifications)
                .HasForeignKey(notification => notification.ReceiverId);

            builder.Entity<Notification>()
                .HasOne(notification => notification.Coin)
                .WithMany(coin => coin.Notifications)
                .HasForeignKey(notification => notification.CoinId);
        }
    }
}