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
                    .Include(coin => coin.Notifications)
                    .Where(coin => coin.Notifications.Last().CreatedAt >= request.Params.StartDate)
                    .ProjectTo<CoinDto>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUserName() })
                    .AsQueryable();

                if (request.Params.IsAscending)
                {
                    query = query.OrderBy(coin => coin.Code);
                }

                if (!string.IsNullOrEmpty(request.Params.CoinName))
                {
                    query = query.Where(coin => coin.DisplayName.Contains(request.Params.CoinName));
                }

                var pagedList = await PagedList<CoinDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize);
                
                return Result<PagedList<CoinDto>>.Success(pagedList);
            }
        }
    }
}