using System.Text.Json.Serialization;

namespace Application.Profiles
{
    public class UserCoinDto
    {
        public Guid Id { get; set; }
        
        public string Identifier { get; set; }

        public string DisplayName { get; set; }

        public string Code { get; set; }
        
        public string Subscriber { get; set; }

        [JsonIgnore]
        public int CommentsCount { get; set; }

        [JsonIgnore]
        public int SubscribersCount { get; set; }
    }
}