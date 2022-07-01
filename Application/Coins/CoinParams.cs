using Application.Core;

namespace Application.Coins
{
    public class CoinParams : PagingParams
    {
        public string CoinName { get; set; }

        public bool IsAscending { get; set; }

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
    }
}