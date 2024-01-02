using System.Net;
using QuickJob.Notifications.DataModel.Configuration;
using QuickJob.Notifications.DataModel.Exceptions;
using IConfigurationProvider = Vostok.Configuration.Abstractions.IConfigurationProvider;

namespace QuickJob.Notifications.Api.Middlewares;

internal sealed class ApiKeyAuthMiddleware :  IMiddleware
{ 
    private readonly HashSet<string> AllowedApiKeys;

    public ApiKeyAuthMiddleware(IConfigurationProvider configurationProvider) => 
        AllowedApiKeys = configurationProvider.Get<ServiceSettings>().AllowedApiKeys;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        CheckAccess(context);
        await next.Invoke(context);
    }
    
    private void CheckAccess(HttpContext context)
    {
        var value = context.Request.Headers.Authorization.FirstOrDefault();
        var apiKey = value?.Replace("api.key ", string.Empty);
        
        if (apiKey is null)
            throw new CustomHttpException(HttpStatusCode.Unauthorized, HttpErrors.UnauthorizedApi);
        
        var hasAccess = AllowedApiKeys.Contains(apiKey);
        if (!hasAccess)
            throw new CustomHttpException(HttpStatusCode.Forbidden, HttpErrors.NoAccessToApi);
    }
}

