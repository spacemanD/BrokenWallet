using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments
{
    public class Create
    {
        public class Command : IRequest<Result<CommentDto>>
        {
            public string Body { get; set; }

            public Guid CoinId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(command => command.Body).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Result<CommentDto>>
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

            public async Task<Result<CommentDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var coin = await _context.Coins.FindAsync(new object[] { request.CoinId }, cancellationToken);

                if (coin == null)
                {
                    return null!;
                }

                var user = await _context.Users
                    .Include(user => user.Photos)
                    .SingleOrDefaultAsync(user => user.UserName == _userAccessor.GetUserName(), cancellationToken);

                var comment = new Comment
                {
                    Author = user,
                    Coin = coin,
                    Body = request.Body
                };

                coin.Comments.Add(comment);

                var suceeded = await _context.SaveChangesAsync(cancellationToken) > 0;

                return suceeded
                    ? Result<CommentDto>.Success(_mapper.Map<CommentDto>(comment))
                    : Result<CommentDto>.Failure("Failed to add the comment");
            }
        }
    }
}