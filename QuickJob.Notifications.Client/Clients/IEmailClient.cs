using System.Threading.Tasks;
using QuickJob.Notifications.Client.Models;
using QuickJob.Notifications.Client.Models.API.Requests.Email;

namespace QuickJob.Notifications.Client.Clients;

public interface IEmailClient
{
    Task<ApiResult> SendEmailAsync(SendEmailRequest request);
}
