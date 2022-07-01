using Application.Core;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Coins
{
    public class UpdateFollowing
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var coin = await _context.Coins
                    .Include(currency => currency.Followers)
                    .ThenInclude(currency => currency.AppUser)
                    .SingleOrDefaultAsync(currency => currency.Id == request.Id, cancellationToken);

                if (coin == null)
                {
                    return null!;
                }

                var user = await _context.Users
                    .FirstOrDefaultAsync(user => user.UserName == _userAccessor.GetUserName(), cancellationToken);

                if (user == null)
                {
                    return null!;
                }

                var attendance = coin.Followers
                    .FirstOrDefault(coinFollowing => coinFollowing.AppUser.UserName == user.UserName);

                if (attendance != null)
                {
                    coin.Followers.Remove(attendance);
                }

                if (attendance == null)
                {
                    attendance = new CoinFollowing
                    {
                        AppUser = user,
                        Coin = coin
                    };

                    coin.Followers.Add(attendance);
                }

                var suceeded = await _context.SaveChangesAsync(cancellationToken) > 0;

                return suceeded
                    ? Result<Unit>.Success(Unit.Value)
                    : Result<Unit>.Failure("Problem updating following status");
            }
        }
    }
}