using QuickJob.Notifications.Api.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSettings();
builder.Services.AddApiKeyAuthMiddleware();
builder.Services.AddExternalServices();
builder.Services.AddRabbitMq();
builder.Services.AddServiceCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddServiceSwaggerDocument();
builder.Services.AddSystemServices();

var app = builder.Build();

//app.UseDeveloperExceptionPage()
app.UseUnhandledExceptionMiddleware();
app.UseSwaggerUi().UseOpenApi();
app.UseHttpsRedirection();
app.UseRouting();
app.UseServiceCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseApiKeyAuthMiddleware();
app.MapControllers();

app.Run();