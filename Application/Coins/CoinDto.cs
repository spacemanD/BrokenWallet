using System.Text.Json.Serialization;

namespace Application.Coins
{
    public class CoinDto
    {
        public Guid Id { get; set; }

        public string Identifier { get; set; }

        public string DisplayName { get; set; }

        public string Code { get; set; }

        [JsonIgnore]
        public int MessagesCount { get; set; }
        public ICollection<FollowerDto> Followers { get; set; }
    }
}