using Microsoft.AspNetCore.Mvc;
using QuickJob.Notifications.BusinessLogic.Services.Interfaces;
using QuickJob.Notifications.DataModel.API.Requests.Email;

namespace QuickJob.Notifications.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailController : ControllerBase
{
    private readonly IMessagesService _messagesService;

    public EmailController(IMessagesService messagesService) => 
        this._messagesService = messagesService;

    [HttpPost]
    public async Task<IActionResult> SendEmail(SendEmailRequest request)
    {
        await _messagesService.SendEmailMessage(request);
        return Ok();
    }
}
