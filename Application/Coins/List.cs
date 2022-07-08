using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Coins
{
    public class List
    {
        public class Query : IRequest<Result<PagedList<CoinDto>>>
        {
            public CoinParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<CoinDto>>>
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

            public async Task<Result<PagedList<CoinDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.Coins
                    .Include(coin => coin.Comments)
                    .Include(coin => coin.Followers)
                    .ProjectTo<CoinDto>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUserName() })
                    .AsQueryable();

                query = request.Params.Predicate switch
                {
                    "popular" => query.OrderByDescending(coin => coin.Followers.Count),
                    "trending" => query.OrderByDescending(coin => coin.MessagesCount),
                    _ => query
                };

                if (!string.IsNullOrEmpty(request.Params.CoinName))
                {
                    query = query.Where(coin =>
                        coin.DisplayName.Contains(request.Params.CoinName));
                }

                var pagedList = await PagedList<CoinDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize);

                return Result<PagedList<CoinDto>>.Success(pagedList);
            }
        }
    }
}