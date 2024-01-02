using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
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
       cfg.Host("localhost",  "/", host =>
       {
           host.Username("guest");
           host.Password("guest");
       });
       cfg.ConfigureEndpoints(context);
   });
});

var app = builder.Build();

app.Run(); 