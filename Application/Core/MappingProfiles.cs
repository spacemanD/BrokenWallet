using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using API.DTOs;
using Application.Activities;
using Application.Comments;
using Application.Profiles;
using Domain;
using Profile = AutoMapper.Profile;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            string currentUsername = null;
            CreateMap<Activity, Activity>();
            CreateMap<Activity, ActivityDto>()
                .ForMember(x => x.HostUsername, o => o.MapFrom(s => s.Attendees
                    .FirstOrDefault(x => x.IsHost).AppUser.UserName));
                    
            CreateMap<ActivityAttendee, AtendeeDto>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
                .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio))
                .ForMember(x => x.Image,
                    o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(x => x.FollowersCount, a => a.MapFrom(s => s.AppUser.Followers.Count))
                .ForMember(x => x.FollowingCount, a => a.MapFrom(s => s.AppUser.Followings.Count))
                .ForMember(x => x.Following,
                    o => o.MapFrom(x => x.AppUser.Followers.Any(x => x.Observer.UserName == currentUsername)));;

            CreateMap<AppUser, Profiles.Profile>()
                .ForMember(x => x.Image, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(x => x.FollowersCount, a => a.MapFrom(s => s.Followers.Count))
                .ForMember(x => x.FollowingCount, a => a.MapFrom(s => s.Followings.Count))
                .ForMember(x => x.Following,
                 o => o.MapFrom(x => x.Followers.Any(x => x.Observer.UserName == currentUsername)));

            CreateMap<ProfileDto, AppUser>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.DisplayName))
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.Username))
                .ForMember(d => d.Bio, o => o.MapFrom(s => s.Bio));

            CreateMap<Activity, UserActivityDto>()            
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Category))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.HostUsername, o => o.MapFrom(s => s.Attendees.FirstOrDefault(x => x.AppUser.UserName == currentUsername)))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Date));

            CreateMap<Comment, CommentDto>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.Author.DisplayName))
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(x => x.Image, o => o.MapFrom(s => s.Author.Photos.FirstOrDefault(x => x.IsMain).Url));

            CreateMap<ActivityAttendee, UserActivityDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Activity.Id))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Activity.Date))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Activity.Title))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Activity.Category))
                .ForMember(d => d.HostUsername, o => o.MapFrom(s => 
                    s.Activity.Attendees.FirstOrDefault(x => x.IsHost).AppUser.UserName));
        }
    }
}
