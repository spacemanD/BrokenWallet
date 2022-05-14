using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class ListActivities
    {
        
        public class Query : IRequest<Result<List<UserActivityDto>>>
        {
            public string? Predicate { get; set; }
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query,Result<List<UserActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper
                ,IUserAccessor userAccessor)
            {
                _context = context;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }
            public async Task<Result<List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var profileActivities = new List<UserActivityDto>();
                switch (request.Predicate)
                {
                    case "past":
                        profileActivities = await _context.Activities.Where(x => x.Attendees.Any(x => x.AppUser.UserName == request.Username) 
                            && x.Date <= DateTime.UtcNow) 
                            .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider,
                                new {currentUsername = _userAccessor.GetUserName()})
                            .ToListAsync();
                            break;
                    case "hosting":
                        profileActivities = await _context.Activities.Where(x => x.Attendees.Any(x => x.AppUser.UserName == request.Username 
                            && x.IsHost)) 
                            .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider,
                                new {currentUsername = _userAccessor.GetUserName()})
                            .ToListAsync();
                            break;
                    default: 
                        profileActivities = await _context.Activities.Where(x => x.Attendees.Any(x => x.AppUser.UserName == request.Username) 
                            && x.Date >= DateTime.UtcNow) 
                            .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider,
                                new {currentUsername = _userAccessor.GetUserName()})
                            .ToListAsync();
                            break;             
                }
                
                return Result<List<UserActivityDto>>.Success(profileActivities);
            }
        }
    }
}