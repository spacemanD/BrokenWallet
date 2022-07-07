using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    public class SetMain
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .Include(user => user.Photos)
                    .FirstOrDefaultAsync(user => user.UserName == _userAccessor.GetUserName(), cancellationToken);

                if (user == null)
                {
                    return null!;
                }

                var photo = user.Photos.FirstOrDefault(photo => photo.Id == request.Id);

                if (photo == null)
                {
                    return null!;
                }

                var currentMain = user.Photos.FirstOrDefault(image => image.IsMain);

                if (currentMain != null)
                {
                    currentMain.IsMain = false;
                }

                photo.IsMain = true;

                var succeeded = await _context.SaveChangesAsync(cancellationToken) > 0;

                return succeeded
                    ? Result<Unit>.Success(Unit.Value)
                    : Result<Unit>.Failure("Problem setting main photo");
            }
        }
    }
}