using QuickJob.Notifications.DataModel.API.Requests.Email;

namespace QuickJob.Notifications.BusinessLogic.Services.Interfaces;

public interface IMessagesService
{
    Task SendEmailMessage(SendEmailRequest request);
}