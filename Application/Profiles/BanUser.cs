using Application.Core;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class BanUser
    {
        public class Command : IRequest<Result<Unit>>
        {
            public AppUser Profile { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var profile = await _context.Users.
                    FirstOrDefaultAsync(user => user.UserName == request.Profile.UserName, cancellationToken);

                if (profile == null)
                {
                    Result<Unit>.Failure("Failed to update the profile");
                }

                profile!.IsBanned = request.Profile.IsBanned;
                
                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                return !result ? Result<Unit>.Failure("Failed to update the profile") : Result<Unit>.Success(Unit.Value);
            }
        }
    }
}