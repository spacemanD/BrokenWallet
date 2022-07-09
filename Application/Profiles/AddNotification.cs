using Application.Core;
using Application.Interfaces;
using MediatR;

namespace Application.Profiles
{
    public class AddNotification
    {
        public class Command : IRequest<Result<NotificationDto>>
        {
        }

        public class Handler : IRequestHandler<Command, Result<NotificationDto>>
        {
            private readonly INotificationBuilder _notificationBuilder;
        
            public Handler(INotificationBuilder notificationBuilder)
            {
                _notificationBuilder = notificationBuilder;
            }

            public async Task<Result<NotificationDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var notification = await _notificationBuilder.BuildAsync();

                if (notification == null)
                {
                    Result<Unit>.Failure("Failed to update the profile subscription");
                }

                return Result<NotificationDto>.Success(notification!);
            }
        }
    }
}