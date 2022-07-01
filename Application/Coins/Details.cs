using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Coins
{
    public class Details
    {
        public class Query : IRequest<Result<CoinDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<CoinDto>>
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

            public async Task<Result<CoinDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var coin = await _context.Coins
                    .ProjectTo<CoinDto>(_mapper.ConfigurationProvider,
                        new { currentUsername = _userAccessor.GetUserName() })
                    .FirstOrDefaultAsync(coin => coin.Id == request.Id, cancellationToken);

                return Result<CoinDto>.Success(coin!);
            }
        }
    }
}