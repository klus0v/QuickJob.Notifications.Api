namespace QuickJob.Notifications.Core.Services.Interfaces;

public interface INotificationService<in TMessage>
{
    public Task SendMessage(TMessage request);
}