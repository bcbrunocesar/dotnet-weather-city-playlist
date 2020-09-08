using Ingaia.Challenge.WebApi.Infrastructure.Enums;
using Ingaia.Challenge.WebApi.Infrastructure.Models;
using System.Collections.Generic;

namespace Ingaia.Challenge.WebApi.Infrastructure.Notificator
{
    public interface INotificator
    {        
        bool HasNotification();
        void Handle(string message, ENotificationType notificationType = ENotificationType.Business);
        IReadOnlyList<NotificationModel> GetNotifications();
        IEnumerable<string> GetMessages();
        string GetSuccessNotification();
    }
}
