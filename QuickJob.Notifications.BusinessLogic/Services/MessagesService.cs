using MassTransit;
using QuickJob.Notifications.BusinessLogic.Mappers;
using QuickJob.Notifications.BusinessLogic.Services.Interfaces;
using QuickJob.Notifications.Core.Storages.Interfaces;
using QuickJob.Notifications.DataModel.API.Requests.Email;
using QuickJob.Notifications.DataModel.Constants;

namespace QuickJob.Notifications.BusinessLogic.Services;

public sealed class MessagesService : IMessagesService
{
    private readonly ITemplatesStorage templatesStorage;
    private readonly IBus bus;

    public MessagesService(IBus bus, ITemplatesStorage templatesStorage)
    {
        this.bus = bus;
        this.templatesStorage = templatesStorage;
    }

    public async Task SendEmailMessage(SendEmailRequest request)
    {
        var template = await templatesStorage.Get(request.TemplateName + TemplateType.Email);
        await bus.Publish(request.ToEvent(template.Id));
    }
}