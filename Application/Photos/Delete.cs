using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IPhotoAccessor _photoAccessor;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
            {
                _context = context;
                _photoAccessor = photoAccessor;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .Include(user => user.Photos)
                    .FirstOrDefaultAsync(user => user.UserName == _userAccessor.GetUserName(), cancellationToken);

                var photo = user?.Photos.FirstOrDefault(photo => photo.Id == request.Id);

                if (user == null)
                {
                    return null!;
                }

                if (photo == null)
                {
                    return null!;
                }

                if (photo.IsMain)
                {
                    return Result<Unit>.Failure("You cannot delete your main photo");
                }

                var result = await _photoAccessor.DeletePhoto(photo.Id);

                if (result == null)
                {
                    return Result<Unit>.Failure("Problem with deleting photo from Cloudinary");
                }

                user.Photos.Remove(photo);

                var deletingSuceeded = await _context.SaveChangesAsync(cancellationToken) > 0;

                return deletingSuceeded
                    ? Result<Unit>.Success(Unit.Value)
                    : Result<Unit>.Failure("Problem with deleting photo from API");
            }
        }
    }
}