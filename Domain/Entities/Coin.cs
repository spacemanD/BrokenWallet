namespace Domain.Entities
{
    public class Coin
    {
        public Guid Id { get; set; }
        
        public string DisplayName { get; set; }
        
        public string Code { get; set; }

        public ICollection<CoinFollowing> Followers { get; set; } = new List<CoinFollowing>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}