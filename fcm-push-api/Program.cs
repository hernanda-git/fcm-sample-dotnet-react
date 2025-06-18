using FcmPushApi.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<FcmService>();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.MapNotificationEndpoints();

app.UseHttpsRedirection();
app.Run();

// Request models
public record NotificationRequest(string Token, string Title, string Body, Dictionary<string, string>? Data = null);
public record BulkNotificationRequest(List<string> Tokens, string Title, string Body, Dictionary<string, string>? Data = null);
public record TopicNotificationRequest(string Topic, string Title, string Body, Dictionary<string, string>? Data = null);