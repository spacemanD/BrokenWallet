using Application.Core;
using Application.Interfaces;
using MediatR;

namespace Application.Profiles
{
    public class AddNotification
    {
        public class Command : IRequest<Result<Unit>>
        {
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly INotificationBuilder _notificationBuilder;
        
            public Handler(INotificationBuilder notificationBuilder)
            {
                _notificationBuilder = notificationBuilder;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var notification = await _notificationBuilder.BuildAsync();

                if (notification == null)
                {
                    Result<Unit>.Failure("Failed to update the profile subscription");
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}