namespace QuickJob.Notifications.DataModel.Entities.Mongo;

public abstract class BaseEntity
{
    public string Id { get; set; }

    public DateTime CreateDateTime { get; set; }
    
    public DateTime? EditDateTime { get; set; }
}