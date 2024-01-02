using Microsoft.AspNetCore.Mvc;
using QuickJob.Notifications.BusinessLogic.Services.Interfaces;
using QuickJob.Notifications.DataModel.API.Requests.Email;

namespace QuickJob.Notifications.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailController : ControllerBase
{
    private readonly IEventsService eventsService;

    public EmailController(IEventsService eventsService) => 
        this.eventsService = eventsService;

    [HttpPost]
    public async Task<IActionResult> SendEmail(SendEmailRequest request)
    {
        await eventsService.SendEmailEvent(request);
        return Ok();
    }
}
