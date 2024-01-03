using System;
using System.Net.Http;
using System.Net.Http.Headers;
using QuickJob.Notifications.Client.Clients;

namespace QuickJob.Notifications.Client;

public class QuickJobNotificationsClient : IQuickJobNotificationsClient
{
    public QuickJobNotificationsClient(string baseUrl, string apiKey)
    {
        var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl)};
        httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("api.key", apiKey);
        var requestSender = new StandaloneRequestSender(httpClient);
        
        Email = new EmailClient(requestSender);
    }

    public IEmailClient Email { get; }
}
