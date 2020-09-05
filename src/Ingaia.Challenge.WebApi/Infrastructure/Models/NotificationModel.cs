using Ingaia.Challenge.WebApi.Infrastructure.Enums;

namespace Ingaia.Challenge.WebApi.Infrastructure.Models
{
    public class NotificationModel
    {
        public NotificationModel(string message, ENotificationType notificationType)
        {
            Message = message;
            NotificationType = notificationType;
        }

        public string Message { get; }
        public ENotificationType NotificationType { get; set; }
    }
}
