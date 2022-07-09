using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class ListUsers
    {
        public class Query : IRequest<Result<List<UserAdminDto>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<UserAdminDto>>>
        {
            private readonly DataContext _context;

            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<UserAdminDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = await _context.Users
                    .ProjectTo<UserAdminDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return Result<List<UserAdminDto>>.Success(users);
            }
        }
    }
}