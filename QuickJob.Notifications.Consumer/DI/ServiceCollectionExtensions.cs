using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using QuickJob.Notifications.Core.Factories;
using QuickJob.Notifications.Core.Services;
using QuickJob.Notifications.Core.Services.Interfaces;
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

namespace QuickJob.Notifications.Consumer.DI;

internal static class ServiceCollectionExtensions
{
    public static void AddSettings(this IServiceCollection services)
    {
        var provider = new ConfigurationProvider();
        
        provider.SetupSourceFor<ServiceSettings>(new JsonFileSource($"QuickJob.Settings/{nameof(ServiceSettings)}.json"));
        provider.SetupSourceFor<SmtpSettings>(new JsonFileSource($"QuickJob.Settings/{nameof(SmtpSettings)}.json"));
        provider.SetupSourceFor<MongoSettings>(new JsonFileSource($"QuickJob.Settings/{nameof(MongoSettings)}.json"));
        provider.SetupSourceFor<RabbitMQSettings>(new JsonFileSource($"QuickJob.Settings/{nameof(RabbitMQSettings)}.json"));
        
        services.AddSingleton<IConfigurationProvider>(provider);
    }

    public static void AddSystemServices(this IServiceCollection services) => services
        .AddDistributedMemoryCache()
        .AddSingleton<IEmailService, EmailService>()
        .AddSingleton<ITelegramService, TelegramService>()
        .AddSingleton<ITemplatesStorage, TemplatesStorage>()
        .AddSingleton<IMongoProvider, MongoProvider>();
    
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
        services
            .AddSingleton<SmtpClientFactory>()
            .TryAddSingleton(x => x.GetRequiredService<SmtpClientFactory>().GetClient());
    }

    public static void AddRabbitMq(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var rabbitMqSettings = serviceProvider.GetRequiredService<IConfigurationProvider>().Get<RabbitMQSettings>();        

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.SetInMemorySagaRepositoryProvider();

            var assembly = typeof(Program).Assembly;

            x.AddConsumers(assembly);
            x.AddSagaStateMachines(assembly);
            x.AddSagas(assembly);
            x.AddActivities(assembly);
   
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqSettings.HostName,  "/", host =>
                {
                    host.Username(rabbitMqSettings.UserName);
                    host.Password(rabbitMqSettings.Password);
                });
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
