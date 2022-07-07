using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class ListNotifications
    {
        public class Query : IRequest<Result<List<NotificationDto>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<NotificationDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _context = context;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }

            public async Task<Result<List<NotificationDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(user => user.UserName == _userAccessor.GetUserName(), cancellationToken);

                if (user == null)
                {
                    return null!;
                }

                var query = _context.Notifications
                    .Where(notification => user.CoinFollowings
                        .Select(following => following.Coin.Id)
                        .Contains(notification.Coin.Id))
                    .OrderByDescending(notification => notification.CreatedAt)
                    .ProjectTo<NotificationDto>(_mapper.ConfigurationProvider);

                var notification = await query.ToListAsync(cancellationToken);

                return Result<List<NotificationDto>>.Success(notification);
            }
        }
    }
}