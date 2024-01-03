using QuickJob.Notifications.DataModel.API.Requests.Email;
using QuickJob.Notifications.DataModel.Entities.RabbitMQ;

namespace QuickJob.Notifications.BusinessLogic.Mappers;

public static class RabbitMqEventsMapper
{
    public static SendEmailEvent ToEvent(this SendEmailRequest request, string templateId) => 
        new(request.Email, templateId, request.Variables);
}