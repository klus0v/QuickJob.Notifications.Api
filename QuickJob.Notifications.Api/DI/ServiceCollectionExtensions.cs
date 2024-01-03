using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using QuickJob.Notifications.Api.Middlewares;
using QuickJob.Notifications.BusinessLogic.Services;
using QuickJob.Notifications.BusinessLogic.Services.Interfaces;
using QuickJob.Notifications.Core.Factories;
using QuickJob.Notifications.Core.Storages;
using QuickJob.Notifications.Core.Storages.Interfaces;
using QuickJob.Notifications.Core.Storages.Mongo;
using QuickJob.Notifications.DataModel.Configuration;
using QuickJob.Users.Client;
using Vostok.Configuration.Sources.Json;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Console;
using Vostok.Logging.File;
using Vostok.Logging.File.Configuration;
using ConfigurationProvider = Vostok.Configuration.ConfigurationProvider;
using IConfigurationProvider = Vostok.Configuration.Abstractions.IConfigurationProvider;

namespace QuickJob.Notifications.Api.DI;

internal static class ServiceCollectionExtensions
{
    private const string FrontSpecificOrigins = "_frontSpecificOrigins";

    public static void AddServiceCors(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var serviceSettings = serviceProvider.GetRequiredService<IConfigurationProvider>().Get<ServiceSettings>();

        services
            .AddCors(option => option
                .AddPolicy(FrontSpecificOrigins, builder => builder.WithOrigins(serviceSettings.Origins.ToArray())
                    .AllowAnyMethod()
                    .AllowAnyHeader()));
    }

    public static void AddServiceSwaggerDocument(this IServiceCollection services)
    {
        services.AddSwaggerDocument(doc =>
        {
            doc.Title = "QuickJob.Notifications.Api";
            doc.AddSecurity("api.key", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
            {
                Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: api.key {for your api}."
            });
        });
    }

    public static void AddSettings(this IServiceCollection services)
    {
        var provider = new ConfigurationProvider();
        
        provider.SetupSourceFor<ServiceSettings>(new JsonFileSource($"QuickJob.Settings/{nameof(ServiceSettings)}.json"));
        provider.SetupSourceFor<MongoSettings>(new JsonFileSource($"QuickJob.Settings/{nameof(MongoSettings)}.json"));
        provider.SetupSourceFor<RabbitMQSettings>(new JsonFileSource($"QuickJob.Settings/{nameof(RabbitMQSettings)}.json"));
        
        services.AddSingleton<IConfigurationProvider>(provider);
    }

    public static void AddSystemServices(this IServiceCollection services) => services
        .AddDistributedMemoryCache()
        .AddSingleton<IMessagesService, MessagesService>()
        .AddSingleton<ITemplatesStorage, TemplatesStorage>()
        .AddSingleton<IMongoProvider, MongoProvider>();

    public static void UseServiceCors(this IApplicationBuilder builder) => 
        builder.UseCors(FrontSpecificOrigins);

    public static void AddExternalServices(this IServiceCollection services)
    {
        services
            .AddSingleton<ILog>(new CompositeLog(new ConsoleLog(), new FileLog(new FileLogSettings())));
        services
            .AddSingleton<UsersClientFactory>()
            .TryAddSingleton<IQuickJobUsersClient>(x => x.GetRequiredService<UsersClientFactory>().GetClient());
        services
            .AddSingleton<MongoFactory>()
            .TryAddSingleton(x => x.GetRequiredService<MongoFactory>().GetClient());
    }

    public static void AddRabbitMq(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var rabbitMqSettings = serviceProvider.GetRequiredService<IConfigurationProvider>().Get<RabbitMQSettings>();        

        services.AddMassTransit(x => x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(rabbitMqSettings.HostName,  "/", host =>
            {
                host.Username(rabbitMqSettings.UserName);
                host.Password(rabbitMqSettings.Password);
            });
            cfg.ConfigureEndpoints(context);
        }));
    }

    public static void UseUnhandledExceptionMiddleware(this IApplicationBuilder builder) => 
        builder.UseMiddleware<UnhandledExceptionMiddleware>();
    
    public static void AddApiKeyAuthMiddleware(this IServiceCollection services) =>
        services.AddSingleton<ApiKeyAuthMiddleware>();

    public static void UseApiKeyAuthMiddleware(this IApplicationBuilder builder) =>
        builder.UseMiddleware<ApiKeyAuthMiddleware>();
}
