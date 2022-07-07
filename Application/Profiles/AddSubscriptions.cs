using Application.Core;
using Application.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class AddSubscriptions
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int CoinId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _accessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor accessor)
            {
                _context = context;
                _mapper = mapper;
                _accessor = accessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var profile = await _context.Users
                    .Include(x=> x.Subscription)
                    .FirstOrDefaultAsync(user => user.UserName == _accessor.GetUserName(), cancellationToken);

                if (profile == null)
                {
                    Result<Unit>.Failure("Failed to update the profile subscription");
                }

                var subscription = await _context.Subscriptions.FirstOrDefaultAsync(x => x.Id == request.CoinId);

                if (subscription == null)
                {
                    Result<Unit>.Failure("Failed to update the profile subscription");
                }

                profile.Subscription = subscription;

                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                return !result ? Result<Unit>.Failure("Failed to update the profile") : Result<Unit>.Success(Unit.Value);
            }
        }
    }
}