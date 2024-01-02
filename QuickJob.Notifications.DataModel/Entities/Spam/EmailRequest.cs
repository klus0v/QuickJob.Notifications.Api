namespace QuickJob.Notifications.DataModel.Entities.Spam;

public class EmailRequest
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; }
}