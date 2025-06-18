using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace FcmPushApi.Services;

public class FcmService
{
    private readonly FirebaseMessaging _messaging;

    public FcmService(IConfiguration configuration)
    {
        var serviceAccountPath = configuration["Firebase:ServiceAccountPath"];
        if (string.IsNullOrEmpty(serviceAccountPath))
        {
            throw new ArgumentException("Service account path is not configured.");
        }

        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(serviceAccountPath)
            });
        }

        _messaging = FirebaseMessaging.DefaultInstance;
    }

    public async Task<string> SendNotificationAsync(string token, string title, string body, Dictionary<string, string>? data = null)
    {
        try
        {
            var message = new Message
            {
                Token = token,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = data
            };

            var response = await _messaging.SendAsync(message);
            return response;
        }
        catch (FirebaseMessagingException ex)
        {
            throw new Exception($"Failed to send notification: {ex.Message}", ex);
        }
    }

    public async Task<BatchResponse> SendBulkNotificationAsync(List<string> tokens, string title, string body, Dictionary<string, string>? data = null)
    {
        try
        {
            var messages = tokens.Select(token => new Message
            {
                Token = token,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = data
            }).ToList();

            var response = await _messaging.SendEachAsync(messages);
            return response;
        }
        catch (FirebaseMessagingException ex)
        {
            throw new Exception($"Failed to send bulk notification: {ex.Message}", ex);
        }
    }
}