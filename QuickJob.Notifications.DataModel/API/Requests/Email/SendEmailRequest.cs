using System.ComponentModel.DataAnnotations;

namespace QuickJob.Notifications.DataModel.API.Requests.Email;

public class SendEmailRequest
{
    [EmailAddress, Required]
    public string Email { get; set; }
    
    [Required]
    public string TemplateId { get; set; }
    
    public Dictionary<string, string>? Variables { get; set; }
}