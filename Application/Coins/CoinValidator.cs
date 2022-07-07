using Domain.Entities;
using FluentValidation;

namespace Application.Coins
{
    public class CoinValidator : AbstractValidator<Coin>
    {
        public CoinValidator()
        {
            RuleFor(coin => coin.Identifier).NotEmpty();
            RuleFor(coin => coin.DisplayName).NotEmpty();
            RuleFor(coin => coin.Code).NotEmpty();
        }
    }
}