namespace QuickJob.Notifications.DataModel.Entities.RabbitMQ;

public record SendEmailEvent(
    string Email, 
    string TemplateName, 
    Dictionary<string, string>? Variables
    ) : BaseRabbitEvent;