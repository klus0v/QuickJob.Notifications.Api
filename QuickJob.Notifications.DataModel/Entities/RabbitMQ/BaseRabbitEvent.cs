namespace QuickJob.Notifications.DataModel.Entities.RabbitMQ;

public abstract record BaseRabbitEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
}