using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Activities;
using Application.Core;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public ProfileDto Profile { get; set; }
        }

        public class CommandValidator: AbstractValidator<Command>
        {
            public CommandValidator() 
            { 
                RuleFor(x => x.Profile).SetValidator(new ProfileValidator());
            }
        }

        public class Handler : IRequestHandler<Edit.Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var profile = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.Profile.Username, 
                    cancellationToken: cancellationToken);

                if(profile == null)  return Result<Unit>.Failure("Failed to update the profile");

                _mapper.Map(request.Profile, profile);
                
                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                return !result ? Result<Unit>.Failure("Failed to update the profile") : Result<Unit>.Success(Unit.Value);
            }
        }
    }
}

        