using Application.Core;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class ListSubscriptions
    {
        public class Query : IRequest<Result<List<Subscription>>>
        {
        }

        public class Handler : IRequestHandler<Query,Result<List<Subscription>>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _accessor;

            public Handler(DataContext context, IUserAccessor accessor)
            {
                _context = context;
                _accessor = accessor;
            }
            
            public async Task<Result<List<Subscription>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var userName = _accessor.GetUserName();
                var user = _context.Users.Include(user => user.Subscription).FirstOrDefault(x => x.UserName == userName);

                if (user == null)
                {
                    return Result<List<Subscription>>.Success(await _context.Subscriptions.ToListAsync(cancellationToken));
                }

                var query = _context.Subscriptions.Select(subscription => new Subscription
                {
                    Id = subscription.Id,
                    Name = subscription.Name,
                    Description = subscription.Description,
                    Price = subscription.Price,
                    Duration = subscription.Duration,
                    IsDefault = user.Subscription.Id == subscription.Id
                });

                var coins = await query.ToListAsync(cancellationToken);

                return Result<List<Subscription>>.Success(coins);
            }
        }
    }
}