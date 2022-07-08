using Application.Core;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class ListUsers
    {
        public class Query : IRequest<Result<List<AppUser>>>
        {
        }

        public class Handler : IRequestHandler<Query,Result<List<AppUser>>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            
            public async Task<Result<List<AppUser>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.Users
                    .AsQueryable();

                var coins = await query.ToListAsync(cancellationToken);

                return Result<List<AppUser>>.Success(coins);
            }
        }
    }
}