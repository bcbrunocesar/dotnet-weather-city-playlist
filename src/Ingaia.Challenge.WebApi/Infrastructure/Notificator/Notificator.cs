using Ingaia.Challenge.WebApi.Infrastructure.Enums;
using Ingaia.Challenge.WebApi.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;

namespace Ingaia.Challenge.WebApi.Infrastructure.Notificator
{
    public class Notificator : INotificator
    {
        private readonly List<NotificationModel> _notifications;

        public Notificator()
        {
            _notifications = new List<NotificationModel>();
        }

        public ENotificationType? GetNotificationType()
        {
            if (!HasNotification()) return null;

            var notifications = _notifications.GroupBy(g => g.NotificationType);
            if (notifications.Count() > 1)
            {
                return ENotificationType.Failed;
            }

            return notifications.FirstOrDefault().Key;
        }

        public bool HasNotification()
        {
            return _notifications.Any();
        }

        public void Handle(string message, ENotificationType notificationType = ENotificationType.Business)
        {
            _notifications.Add(new NotificationModel(message, notificationType));
        }

        public IReadOnlyList<NotificationModel> GetNotifications()
        {
            return _notifications;
        }

        public IEnumerable<string> GetMessages()
        {
            if (!HasNotification()) return default;
            return GetNotifications().Select(x => x.Message);
        }
    }
}
