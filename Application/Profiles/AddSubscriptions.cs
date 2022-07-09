using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class AddSubscriptions
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int SubscriptionId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _accessor;

            public Handler(DataContext context, IUserAccessor accessor)
            {
                _context = context;
                _accessor = accessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var profile = await _context.Users
                    .Include(user => user.Subscription)
                    .FirstOrDefaultAsync(user => user.UserName == _accessor.GetUserName(), cancellationToken);

                if (profile == null)
                {
                    return Result<Unit>.Failure("Failed to update the profile subscription");
                }

                var subscription = await _context.Subscriptions
                    .FirstOrDefaultAsync(subscription =>
                        subscription.Id == request.SubscriptionId, cancellationToken);

                if (subscription == null)
                {
                    return null!;
                }

                profile.Subscription = subscription;

                var suceeded = await _context.SaveChangesAsync(cancellationToken) > 0;

                return suceeded
                    ? Result<Unit>.Success(Unit.Value)
                    : Result<Unit>.Failure("Failed to update the profile");
            }
        }
    }
}