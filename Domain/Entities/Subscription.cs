namespace Domain.Entities
{
    public class Subscription
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public TimeSpan Duration { get; set; }

        public ICollection<AppUser> Subscriber { get; set; }
    }
}