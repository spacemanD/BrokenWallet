using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public TYPE Type { get; set; }
        public DataContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<CoinFollowing> CoinFollowings { get; set; }
        
        public DbSet<Photo> Photos { get; set;}

        public DbSet<Comment> Comments { get; set; }

        public DbSet<UserFollowing> UserFollowings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<CoinFollowing>(entity =>
                    entity.HasKey(options => new {options.AppUserId, options.CoinId}))
            builder.Entity<ActivityAttendee>(x => x.HasKey(aa => new {aa.AppUserId, aa.ActivityId}));

            builder.Entity<ActivityAttendee>()
                .HasOne(u => u.AppUser)
                .WithMany(a => a.Activities)
                .HasForeignKey(aa => aa.AppUserId);

            builder.Entity<ActivityAttendee>()
                .HasOne(u => u.Activity)
                .WithMany(a => a.Attendees)
                .HasForeignKey(aa => aa.ActivityId);

            builder.Entity<Comment>()
                .HasOne(x => x.Activity)
                .WithMany(x => x.Comments)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<UserFollowing>(x => {
                x.HasKey(s => new {s.ObserverId, s.TargetId});
                
                x.HasOne(x => x.Observer)
                    .WithMany(x => x.Followings)
                    .HasForeignKey(s => s.ObserverId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                x.HasOne(x => x.Target)
                    .WithMany(x => x.Followers)
                    .HasForeignKey(s => s.TargetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            builder.Entity<AppUser>()
                .HasOne(user => user.Subscription)
                .WithOne(subscription => subscription.Subscriber)
                .HasForeignKey(subscription => )
        }
    }
}