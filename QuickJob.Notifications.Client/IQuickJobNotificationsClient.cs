
using QuickJob.Notifications.Client.Clients;

namespace QuickJob.Notifications.Client;

public interface IQuickJobNotificationsClient
{
    IEmailClient Email { get; }
}
