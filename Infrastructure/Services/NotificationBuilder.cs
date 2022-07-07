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

        public NotificationBuilder(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<NotificationDto> BuildAsync()
        {
            var notification = new Notification();

            if (!_dataContext.Coins.Any())
            {
                return null!;
            }

            var random = new Random();
            notification.Coin = _dataContext.Coins.ElementAtOrDefault(random.Next() % _dataContext.Coins.Count());

            if (notification.Coin == default)
            {
                return null!;
            }

            notification.Mode = (NotificationMode)(random.Next() % 10 + 1);

            await _dataContext.Notifications.AddAsync(notification);

            return _mapper.Map<NotificationDto>(notification);
        }
    }
}