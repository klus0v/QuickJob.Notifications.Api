namespace QuickJob.Notifications.DataModel.Entities.RabbitMQ;

public record SendEmailEvent(
    string Email, 
    string TemplateId, 
    Dictionary<string, string>? Variables
    ) : BaseRabbitEvent;