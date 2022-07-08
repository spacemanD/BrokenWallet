using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class ListSubscriptions
    {
        public class Query : IRequest<Result<List<SubscriptionDto>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<SubscriptionDto>>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IUserAccessor userAccessor, IMapper mapper)
            {
                _context = context;
                _userAccessor = userAccessor;
                _mapper = mapper;
            }

            public async Task<Result<List<SubscriptionDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var username = _userAccessor.GetUserName();
                var user = _context.Users
                    .Include(user => user.Subscription)
                    .FirstOrDefault(user => user.UserName == username);

                if (user == null)
                {
                    return null!;
                }

                var subscriptions = await _context.Subscriptions
                    .ProjectTo<SubscriptionDto>(_mapper.ConfigurationProvider,
                        new { currentSubscription = user.Subscription.Id })
                    .ToListAsync(cancellationToken);

                return Result<List<SubscriptionDto>>.Success(subscriptions);
            }
        }
    }
}