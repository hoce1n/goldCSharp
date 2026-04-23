using Application.Common.Exceptions;
using Domain.Common.Errors;
using Domain.Exceptions;
using System.Text.Json;
using System.Net;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public ErrorHandlingMiddleware(
        RequestDelegate next, 
        ILogger<ErrorHandlingMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
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
        if (context.Response.HasStarted)
        {
            _logger.LogWarning("Cannot write error response. Response has already started. CorrelationId={CorrelationId}",
                context.Items["CorrelationId"]);

            return;
        }

        var httpCode = HttpStatusCode.InternalServerError;
        var errorCode = ErrorCodes.General.Unexpected;
        var message = "An unexpected error occurred.";
        object? details = null;

        string correlationId = context.Items["CorrelationId"]?.ToString()
                                ?? Guid.NewGuid().ToString();

        switch (exception)
        {
            case DomainException dex:
                httpCode = HttpStatusCode.BadRequest;
                errorCode = dex.Code;
                message = dex.Message;
                break;

            case ValidationException vex:
                httpCode = HttpStatusCode.BadRequest;
                errorCode = vex.Code;
                message = vex.Message;
                details = vex.Errors;
                break;

            case NotFoundException nfe:
                httpCode = HttpStatusCode.NotFound;
                errorCode = nfe.Code;
                message = nfe.Message;
                break;

            case UnauthorizedException uae:
                httpCode = HttpStatusCode.Unauthorized;
                errorCode = uae.Code;
                message = uae.Message;
                break;

            case ForbiddenException fe:
                httpCode = HttpStatusCode.Forbidden;
                errorCode = fe.Code;
                message = fe.Message;
                break;

            default:
                if (_env.IsDevelopment())
                {
                    message = exception.Message;
                }
                break;
        }

        _logger.LogError(exception,
            "Error occurred | Code: {Code} | Message: {Message} | CorrelationId: {CorrelationId}",
            errorCode, message, correlationId);

        var responseBody = new
        {
            success = false,
            error = new
            {
                code = errorCode,
                message = message,
                details = details,
                correlationId = correlationId
            }
        };

        context.Response.StatusCode = (int)httpCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(responseBody));
    }
}
