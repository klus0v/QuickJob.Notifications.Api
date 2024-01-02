using QuickJob.Notifications.DataModel.API.Requests.Email;

namespace QuickJob.Notifications.BusinessLogic.Services.Interfaces;

public interface IEventsService
{
    Task SendEmailEvent(SendEmailRequest request);
}