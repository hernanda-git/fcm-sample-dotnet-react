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

        app.MapPost("/api/notification/topic", async (FcmService fcmService, TopicNotificationRequest request) =>
        {
            if (string.IsNullOrEmpty(request.Topic) || string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Body))
            {
                return Results.BadRequest(new { Error = "Topic, Title, and Body are required" });
            }

            try
            {
                var messageId = await fcmService.SendNotificationToTopicAsync(
                    request.Topic,
                    request.Title,
                    request.Body,
                    request.Data
                );
                return Results.Ok(new { MessageId = messageId, Message = "Notification sent to topic successfully" });
            }
            catch (Exception ex)
            {
                return Results.Json(new { Error = ex.Message }, statusCode: 500);
            }
        })
        .WithName("SendTopicNotification")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Send a FCM notification to a topic";
            operation.Description = "Sends a push notification to all devices subscribed to a topic.";
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
                                ["topic"] = new OpenApiSchema { Type = JsonSchemaType.String, Description = "FCM topic name" },
                                ["title"] = new OpenApiSchema { Type = JsonSchemaType.String, Description = "Notification title" },
                                ["body"] = new OpenApiSchema { Type = JsonSchemaType.String, Description = "Notification body" },
                                ["data"] = new OpenApiSchema
                                {
                                    Type = JsonSchemaType.Object,
                                    AdditionalProperties = new OpenApiSchema { Type = JsonSchemaType.String },
                                    Description = "Optional key-value pairs for additional data"
                                }
                            },
                            Required = new HashSet<string> { "topic", "title", "body" }
                        }
                    }
                }
            };
            return operation;
        });

        // Subscribe to topic endpoint
        app.MapPost("/api/notification/subscribe-topic", async (FcmService fcmService, TopicSubscriptionRequest request) =>
        {
            if (request.Tokens == null || !request.Tokens.Any() || string.IsNullOrEmpty(request.Topic))
            {
                return Results.BadRequest(new { Error = "Tokens and Topic are required" });
            }
            try
            {
                var response = await fcmService.SubscribeToTopicAsync(request.Tokens, request.Topic);
                return Results.Ok(new { response.SuccessCount, response.FailureCount, response.Errors });
            }
            catch (Exception ex)
            {
                return Results.Json(new { Error = ex.Message }, statusCode: 500);
            }
        });

        // Unsubscribe from topic endpoint
        app.MapPost("/api/notification/unsubscribe-topic", async (FcmService fcmService, TopicSubscriptionRequest request) =>
        {
            if (request.Tokens == null || !request.Tokens.Any() || string.IsNullOrEmpty(request.Topic))
            {
                return Results.BadRequest(new { Error = "Tokens and Topic are required" });
            }
            try
            {
                var response = await fcmService.UnsubscribeFromTopicAsync(request.Tokens, request.Topic);
                return Results.Ok(new { response.SuccessCount, response.FailureCount, response.Errors });
            }
            catch (Exception ex)
            {
                return Results.Json(new { Error = ex.Message }, statusCode: 500);
            }
        });
    }
}

public class TopicSubscriptionRequest
{
    public List<string> Tokens { get; set; } = new();
    public string Topic { get; set; } = string.Empty;
}