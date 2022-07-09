using Application.Coins;
using Application.Comments;
using Application.Profiles;
using Domain.Entities;

namespace Application.Core
{
    public class MappingProfiles : AutoMapper.Profile
    {
        private static readonly string[] NotificationMessages =
        {
            "The exchange rate of {0} has plummeted.",
            "{0} exchange rate has stopped falling.",
            "{0} exchange rate continues to fall.",
            "The exchange rate of {0} started to fall.",
            "{0} exchange rate remains at the same level.",
            "The exchange rate of {0} began to rise.",
            "{0} exchange rate continues to rise.",
            "{0} exchange rate has stopped rising.",
            "The exchange rate of {0} has risen sharply."
        };
        
        public MappingProfiles()
        {
            string currentUsername = null!;
            int? currentSubscription = null;

            CreateMap<Coin, Coin>();

            CreateMap<Coin, CoinDto>()
                .ForMember(coinDto => coinDto.MessagesCount, options => options.MapFrom(coin => coin.Comments.Count));

            CreateMap<CoinFollowing, FollowerDto>()
                .ForMember(follower => follower.DisplayName, options => options.MapFrom(following => following.AppUser.DisplayName))
                .ForMember(follower => follower.Username, options => options.MapFrom(following => following.AppUser.UserName))
                .ForMember(follower => follower.Bio, options => options.MapFrom(following => following.AppUser.Bio))
                .ForMember(follower => follower.Image, options => options.MapFrom(following => following.AppUser.Photos.FirstOrDefault(photo => photo.IsMain)!.Url))
                .ForMember(follower => follower.FollowersCount, options => options.MapFrom(following => following.AppUser.Followers.Count))
                .ForMember(follower => follower.FollowingCount, options => options.MapFrom(following => following.AppUser.Followings.Count))
                .ForMember(follower => follower.Following, options => options.MapFrom(following => following.AppUser.Followers.Any(userFollowing => userFollowing.Observer.UserName == currentUsername)))
                .ForMember(follower => follower.IsAdmin, options => options.MapFrom(following => following.AppUser.IsAdmin));

            CreateMap<AppUser, Profile>()
                .ForMember(profile => profile.Image, options => options.MapFrom(user => user.Photos.FirstOrDefault(x => x.IsMain)!.Url))
                .ForMember(profile => profile.FollowersCount, options => options.MapFrom(user => user.Followers.Count))
                .ForMember(profile => profile.FollowingCount, options => options.MapFrom(user => user.Followings.Count))
                .ForMember(profile => profile.Following, options => options.MapFrom(user => user.Followers.Any(following => following.Observer.UserName == currentUsername)));

            CreateMap<ProfileDto, AppUser>()
                .ForMember(user => user.DisplayName, options => options.MapFrom(profile => profile.DisplayName))
                .ForMember(user => user.UserName, options => options.MapFrom(profile => profile.Username))
                .ForMember(user => user.Bio, options => options.MapFrom(profile => profile.Bio));

            CreateMap<Coin, UserCoinDto>()
                .ForMember(userCoinDto => userCoinDto.Id, options => options.MapFrom(coin => coin.Id))
                .ForMember(userCoinDto => userCoinDto.Identifier, options => options.MapFrom(coin => coin.Identifier))
                .ForMember(userCoinDto => userCoinDto.DisplayName, options => options.MapFrom(coin => coin.DisplayName))
                .ForMember(userCoinDto => userCoinDto.Code, options => options.MapFrom(coin => coin.Code))
                .ForMember(userCoinDto => userCoinDto.Subscriber, options => options.MapFrom(coin => coin.Followers.FirstOrDefault(following => following.AppUser.UserName == currentUsername)))
                .ForMember(userCoinDto => userCoinDto.CommentsCount, options => options.MapFrom(coin => coin.Comments.Count))
                .ForMember(userCoinDto => userCoinDto.SubscribersCount, options => options.MapFrom(coin => coin.Followers.Count));

            CreateMap<Comment, CommentDto>()
                .ForMember(comment => comment.DisplayName, options => options.MapFrom(comment => comment.Author.DisplayName))
                .ForMember(comment => comment.Username, options => options.MapFrom(comment => comment.Author.UserName))
                .ForMember(comment => comment.Image, options => options.MapFrom(comment => comment.Author.Photos.FirstOrDefault(photo => photo.IsMain)!.Url));

            CreateMap<CoinFollowing, UserCoinDto>()
                .ForMember(userCoinDto => userCoinDto.Id, options => options.MapFrom(following => following.Coin.Id))
                .ForMember(userCoinDto => userCoinDto.Identifier, options => options.MapFrom(following => following.Coin.Identifier))
                .ForMember(userCoinDto => userCoinDto.DisplayName, options => options.MapFrom(following => following.Coin.DisplayName))
                .ForMember(userCoinDto => userCoinDto.Code, options => options.MapFrom(following => following.Coin.Code))
                .ForMember(userCoinDto => userCoinDto.Subscriber, options => options.MapFrom(following => following.AppUser.UserName))
                .ForMember(userCoinDto => userCoinDto.CommentsCount, options => options.MapFrom(following => following.Coin.Comments.Count))
                .ForMember(userCoinDto => userCoinDto.SubscribersCount, options => options.MapFrom(following => following.Coin.Followers.Count));
            
            CreateMap<Notification, NotificationDto>()
                .ForMember(notificationDto => notificationDto.Message, options => options.MapFrom(notification => string.Format(NotificationMessages[(int)notification.Mode - 1], notification.Coin.DisplayName)));

            CreateMap<AppUser, UserAdminDto>();

            CreateMap<Subscription, SubscriptionDto>()
                .ForMember(subscriptionDto => subscriptionDto.IsDefault, options => options.MapFrom(subscription => subscription.Id == currentSubscription));
        }
    }
}