using Application.Core;

namespace Application.Coins
{
    public class CoinParams : PagingParams
    {
        public string? CoinName { get; set; }

        public string? Predicate { get; set; }
    }
}