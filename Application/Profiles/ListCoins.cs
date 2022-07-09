using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class ListCoins
    {
        public class Query : IRequest<Result<List<UserCoinDto>>>
        {
            public string? Predicate { get; set; }

            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query,Result<List<UserCoinDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            
            public async Task<Result<List<UserCoinDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.CoinFollowings
                    .Where(coinFollowing => coinFollowing.AppUser!.UserName == request.Username)
                    .OrderBy(coinFollowing => coinFollowing.Coin.Code)
                    .ProjectTo<UserCoinDto>(_mapper.ConfigurationProvider)
                    .AsQueryable();

                query = request.Predicate switch
                {
                    "popular" => query.OrderByDescending(userCoin => userCoin.SubscribersCount).Take(10),
                    "trending" => query.OrderByDescending(userCoin => userCoin.CommentsCount).Take(10),
                    _ => query
                };

                var coins = await query.ToListAsync(cancellationToken);

                return Result<List<UserCoinDto>>.Success(coins);
            }
        }
    }
}