using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Coins
{
    public class Delete
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

                var coin = await _context.Coins.FindAsync(new object[] { request.Id }, cancellationToken);

                if (coin == null)
                {
                    return null!;
                }

                _context.Coins.Remove(coin);

                var suceeded = await _context.SaveChangesAsync(cancellationToken) > 0;

                return suceeded
                    ? Result<Unit>.Success(Unit.Value)
                    : Result<Unit>.Failure("Failed to delete the coin");
            }
        }
    }
}