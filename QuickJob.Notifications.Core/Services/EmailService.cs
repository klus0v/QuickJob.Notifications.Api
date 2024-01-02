using System.Net;
using System.Net.Mail;
using QuickJob.Notifications.Core.Services.Interfaces;
using QuickJob.Notifications.DataModel.Configuration;
using QuickJob.Notifications.DataModel.Entities.Spam;
using QuickJob.Notifications.DataModel.Exceptions;
using Vostok.Configuration.Abstractions;
using Vostok.Logging.Abstractions;

namespace QuickJob.Notifications.Core.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings smtpSettings;
    private readonly SmtpClient smtpClient;
    private readonly ILog log;

    public EmailService(SmtpClient smtpClient, IConfigurationProvider configurationProvider, ILog log)
    {
        smtpSettings = configurationProvider.Get<SmtpSettings>();
        this.smtpClient = smtpClient;
        this.log = log;
    }

    public Task SendMessage(EmailRequest request)
    {
        try
        {
            var message = new MailMessage
            {
                From = new MailAddress(smtpSettings.EmailFrom),
                Subject = request.Subject,
                IsBodyHtml = request.IsHtml,
                Body = request.Body
            };
            message.To.Add(request.Email);

            smtpClient.SendAsync(message, request.Id);
            log.Info($"Message to the email: '{request.Email}' has been sent successfully");
        }
        catch (Exception)
        {
            log.Error($"Email: '{request.Email}' send error, event id: '{request.Id}");
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Smtp(request.Id));
        }
        return Task.CompletedTask;
    }
}