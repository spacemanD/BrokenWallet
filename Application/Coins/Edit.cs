using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Coins
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Coin Coin { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(command => command.Coin).SetValidator(new CoinValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
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

                var coin = await _context.Coins.FindAsync(new object[] { request.Coin.Id }, cancellationToken);

                if (coin == null)
                {
                    return null!;
                }

                _mapper.Map(request.Coin, coin);

                var suceeded = await _context.SaveChangesAsync(cancellationToken) > 0;

                return suceeded ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Failed to update the coin");
            }
        }
    }
}