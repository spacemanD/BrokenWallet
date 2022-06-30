namespace Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        
        public string Body { get; set; }
        
        public AppUser Author { get; set; }
        
        public Coin Coin { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}