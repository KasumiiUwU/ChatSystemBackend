using System.Net;
using System.Text.Json;
using ChatSystemBackend.Exceptions;

namespace ChatSystemBackend.Middleware;

public class ExceptionMiddleware
{
    
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        //var message = "An unexpected error occurred.";
        var message = exception.Message;
        var errorCode = "SERVER_ERROR";
        var details = exception.InnerException?.Message;

        switch (exception)
        {
            case CustomExceptions.InvalidDataException:
                statusCode = HttpStatusCode.BadRequest;
                errorCode = "INVALID_DATA";
                break;

            case CustomExceptions.DataNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                errorCode = "DATA_NOT_FOUND";
                break;

            case CustomExceptions.DataExistException:
                statusCode = HttpStatusCode.Conflict;
                errorCode = "DATA_EXISTS";
                break;

            case CustomExceptions.UnAuthorizedException:
                statusCode = HttpStatusCode.Unauthorized;
                errorCode = "UNAUTHORIZED";
                break;

            case CustomExceptions.ForbbidenException:
                statusCode = HttpStatusCode.Forbidden;
                errorCode = "FORBIDDEN";
                break;

            case CustomExceptions.InternalServerErrorException:
                statusCode = HttpStatusCode.InternalServerError;
                errorCode = "INTERNAL_SERVER_ERROR";
                break;

            case AggregateException aggregateEx:
                statusCode = HttpStatusCode.InternalServerError;
                errorCode = "AGGREGATE_EXCEPTION";
                message = string.Join("; ", aggregateEx.InnerExceptions.Select(e => e.Message));
                break;

            default:
                _logger.LogError(exception, "Unhandled exception occurred.");
                break;
        }

        var response = new
        {
            statusCode = (int)statusCode,
            errorCode,
            message,
            details,
            path = context.Request.Path,
            timestamp = DateTime.UtcNow
        };

        _logger.LogError("API Error: {StatusCode} - {ErrorCode} - {Message} - Path: {Path}",
            statusCode, errorCode, message, context.Request.Path);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });

        await context.Response.WriteAsync(jsonResponse);
    }
}