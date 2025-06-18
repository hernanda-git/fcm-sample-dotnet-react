using FcmPushApi.Services;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Models.Interfaces;

public static class NotificationEndpoints
{
    public static void MapNotificationEndpoints(this WebApplication app)
    {
        // Single notification endpoint
        app.MapPost("/api/notification/send", async (FcmService fcmService, NotificationRequest request) =>
        {
            if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Body))
            {
                return Results.BadRequest(new { Error = "Token, Title, and Body are required" });
            }

            try
            {
                var messageId = await fcmService.SendNotificationAsync(
                    request.Token,
                    request.Title,
                    request.Body,
                    request.Data
                );
                return Results.Ok(new { MessageId = messageId, Message = "Notification sent successfully" });
            }
            catch (Exception ex)
            {
                return Results.Json(new { Error = ex.Message }, statusCode: 500);
            }
        })
        .WithName("SendNotification")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Send a single FCM notification";
            operation.Description = "Sends a push notification to a single device using its FCM token.";
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = JsonSchemaType.Object,
                            Properties = new Dictionary<string, IOpenApiSchema>
                            {
                                ["token"] = new OpenApiSchema { Type = JsonSchemaType.String, Description = "FCM registration token" },
                                ["title"] = new OpenApiSchema { Type = JsonSchemaType.String, Description = "Notification title" },
                                ["body"] = new OpenApiSchema { Type = JsonSchemaType.String, Description = "Notification body" },
                                ["data"] = new OpenApiSchema
                                {
                                    Type = JsonSchemaType.Object,
                                    AdditionalProperties = new OpenApiSchema { Type = JsonSchemaType.String },
                                    Description = "Optional key-value pairs for additional data"
                                }
                            },
                            Required = new HashSet<string> { "token", "title", "body" }
                        }
                    }
                }
            };
            return operation;
        });

        // Bulk notification endpoint
        app.MapPost("/api/notification/send-bulk", async (FcmService fcmService, BulkNotificationRequest request) =>
        {
            if (request.Tokens == null || !request.Tokens.Any() || string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Body))
            {
                return Results.BadRequest(new { Error = "Tokens, Title, and Body are required" });
            }

            try
            {
                var response = await fcmService.SendBulkNotificationAsync(
                    request.Tokens,
                    request.Title,
                    request.Body,
                    request.Data
                );
                return Results.Ok(new
                {
                    SuccessCount = response.SuccessCount,
                    FailureCount = response.FailureCount,
                    Message = "Bulk notification sent"
                });
            }
            catch (Exception ex)
            {
                return Results.Json(new { Error = ex.Message }, statusCode: 500);
            }
        })
        .WithName("SendBulkNotification")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Send FCM notifications to multiple devices";
            operation.Description = "Sends push notifications to multiple devices using their FCM tokens.";
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = JsonSchemaType.Object,
                            Properties = new Dictionary<string, IOpenApiSchema>
                            {
                                ["tokens"] = new OpenApiSchema
                                {
                                    Type = JsonSchemaType.Array,
                                    Items = new OpenApiSchema { Type = JsonSchemaType.String },
                                    Description = "List of FCM registration tokens"
                                },
                                ["title"] = new OpenApiSchema { Type = JsonSchemaType.String, Description = "Notification title" },
                                ["body"] = new OpenApiSchema { Type = JsonSchemaType.String, Description = "Notification body" },
                                ["data"] = new OpenApiSchema
                                {
                                    Type = JsonSchemaType.Object,
                                    AdditionalProperties = new OpenApiSchema { Type = JsonSchemaType.String },
                                    Description = "Optional key-value pairs for additional data"
                                }
                            },
                            Required = new HashSet<string> { "tokens", "title", "body" }
                        }
                    }
                }
            };
            return operation;
        });
    }
}