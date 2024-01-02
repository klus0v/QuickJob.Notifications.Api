using MassTransit;
using QuickJob.Notifications.BusinessLogic.Mappers;
using QuickJob.Notifications.BusinessLogic.Services.Interfaces;
using QuickJob.Notifications.DataModel.API.Requests.Email;

namespace QuickJob.Notifications.BusinessLogic.Services;

public sealed class EventsService : IEventsService
{
    private readonly IBus bus;

    public EventsService(IBus bus) =>
        this.bus = bus;
    
    public async Task SendEmailEvent(SendEmailRequest request) => 
        await bus.Publish(request.ToEvent());
}