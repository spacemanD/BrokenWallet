using Application.Interfaces;
using Application.Profiles;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Persistence;

namespace Infrastructure.Services
{
    public class NotificationBuilder : INotificationBuilder
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly Random _random;

        public NotificationBuilder(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _random = new Random();
        }

        public async Task<NotificationDto> BuildAsync()
        {
            var notification = new Notification
            {
                Mode = (NotificationMode)(_random.Next() % 9 + 1)
            };

            if (!_dataContext.Coins.Any())
            {
                return null!;
            }

            notification.Coin = _dataContext.Coins
                .Skip(_random.Next() % _dataContext.Coins.Count())
                .FirstOrDefault();

            if (notification.Coin == default)
            {
                return null!;
            }

            await _dataContext.Notifications.AddAsync(notification);
            await _dataContext.SaveChangesAsync();

            return _mapper.Map<NotificationDto>(notification);
        }
    }
}