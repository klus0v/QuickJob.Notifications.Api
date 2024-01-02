using QuickJob.Notifications.Core.Services.Interfaces;
using QuickJob.Notifications.DataModel.Entities.Telegram;

namespace QuickJob.Notifications.Core.Services;

public class TelegramService : ITelegramService
{
    public Task SendMessage(TelegramRequest request)
    {
        throw new NotImplementedException();
    }
}