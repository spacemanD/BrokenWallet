using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public string Bio { get; set; }
        
        public Guid SubscriptionId { get; set; }

        public Subscription Subscription { get; set; }
        
        public ICollection<Photo> Photos { get; set; }
        
        public ICollection<CoinFollowing> CoinFollowings { get; set; }

        public ICollection<UserFollowing> Followings { get; set; }
        
        public ICollection<UserFollowing> Followers { get; set; }
        
        public ICollection<Notification> Notifications { get; set; }
    }
}