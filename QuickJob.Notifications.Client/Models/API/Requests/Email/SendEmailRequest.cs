using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuickJob.Notifications.Client.Models.API.Requests.Email;

public class SendEmailRequest
{
    [EmailAddress, Required]
    public string Email { get; set; }
    
    [Required]
    public string TemplateName { get; set; }
    
    public Dictionary<string, string>? Variables { get; set; }
}