using Application.Core;
using Application.Interfaces;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Coins
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Coin Coin { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(command => command.Coin).SetValidator(new CoinValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(user => user.UserName == _userAccessor.GetUserName(), cancellationToken);

                if (user == null)
                {
                    return null!;
                }

                if (!user.IsAdmin)
                {
                    return Result<Unit>.Failure("User is not an admin");
                }

                var following = new CoinFollowing
                {
                    AppUser = user,
                    Coin = request.Coin
                };

                request.Coin.Followers.Add(following);

                _context.Coins.Add(request.Coin);

                var suceeded = await _context.SaveChangesAsync(cancellationToken) > 0;

                return suceeded ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Failed to create coin");
            }
        }
    }
}