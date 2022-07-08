namespace Application.Profiles;

public class SubscriptionDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; }

    public bool IsDefault { get; set; }

    public TimeSpan Duration { get; set; }
}