using System.Threading.Tasks;
using QuickJob.Notifications.Client.Models;
using QuickJob.Notifications.Client.Models.API.Requests.Email;

namespace QuickJob.Notifications.Client.Clients;

public class EmailClient : IEmailClient
{
    private readonly IRequestSender sender;

    public EmailClient(IRequestSender sender)
    {
        this.sender = sender;
    }

    public async Task<ApiResult> SendEmailAsync(SendEmailRequest request) => 
        await sender.SendRequestAsync("POST", ClientPaths.Email, request);

}