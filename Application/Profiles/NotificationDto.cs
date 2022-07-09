namespace Application.Profiles
{
    public class NotificationDto
    {
        public int Id { get; set; }
    
        public Guid CoinId { get; set; }
    
        public DateTime CreatedAt { get; set; }

        public string Message { get; set; }
    }
}