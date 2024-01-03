using MassTransit;
using QuickJob.Notifications.Core.Extensions;
using QuickJob.Notifications.Core.Services.Interfaces;
using QuickJob.Notifications.Core.Storages.Interfaces;
using QuickJob.Notifications.DataModel.Constants;
using QuickJob.Notifications.DataModel.Entities.RabbitMQ;
using QuickJob.Notifications.DataModel.Entities.Spam;
using Vostok.Logging.Abstractions;

namespace QuickJob.Notifications.Consumer.Consumers;

public sealed class SendEmailConsumer: IConsumer<SendEmailEvent>
{
    private readonly ILog log;
    private readonly ITemplatesStorage templatesStorage;
    private readonly IEmailService emailService;
    
    public SendEmailConsumer(ILog log, ITemplatesStorage templatesStorage, IEmailService emailService)
    {
        this.templatesStorage = templatesStorage;
        this.emailService = emailService;
        this.log = log.ForContext(nameof(SendEmailConsumer));
    }
    
    public async Task Consume(ConsumeContext<SendEmailEvent> context)
    {
        var emailEvent = context.Message;
        Console.WriteLine(emailEvent.Id + "_" + emailEvent.Email);
        log.Info($"Consume event: {emailEvent.Id}");
        
        var template = await templatesStorage.Get(emailEvent.TemplateName + TemplateType.Email);
        if (emailEvent.Variables is not null)
            template.SetVariables(emailEvent.Variables);
        
        var request = new EmailRequest
        {
            Id = emailEvent.Id,
            Email = emailEvent.Email,
            Subject = template.Title ?? "",
            Body = template.Body,
            IsHtml = template.IsHtml ?? false
        };
        await emailService.SendMessage(request);
    }
}