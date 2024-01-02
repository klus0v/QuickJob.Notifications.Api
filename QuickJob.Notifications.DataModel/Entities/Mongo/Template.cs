namespace QuickJob.Notifications.DataModel.Entities.Mongo;

public class Template : BaseEntity
{
    public string? Title { get; set; }
    public string Body { get; set; }
    public bool? IsHtml { get; set; }
}