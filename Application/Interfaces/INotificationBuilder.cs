using Application.Profiles;

namespace Application.Interfaces
{
    public interface INotificationBuilder
    {
        Task<NotificationDto> BuildAsync();
    }
}