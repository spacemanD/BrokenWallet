using Application.Core;
using Application.Interfaces;
using AutoMapper;
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

                var sortedNotifications = await _context.Notifications
                    .Include(notification => notification.Coin)
                    .ThenInclude(coin => coin.Followers)
                    .OrderByDescending(notification => notification.CreatedAt)
                    .ToListAsync(cancellationToken);

                var filteredNotifications = sortedNotifications
                    .Where(notification => notification.Coin.Followers
                        .Select(followers => followers.AppUserId)
                        .Contains(user.Id));

                var notifications = _mapper.Map<List<NotificationDto>>(filteredNotifications);

                return Result<List<NotificationDto>>.Success(notifications);
            }
        }
    }
}