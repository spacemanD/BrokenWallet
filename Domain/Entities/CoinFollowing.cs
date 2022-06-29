namespace Domain.Entities
{
    public class CoinFollowing
    {
        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }    

        public Guid CoinId { get; set; }

        public Coin Coin { get; set; }
    }
}