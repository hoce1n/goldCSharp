namespace API.Middleware
{
    /// <summary>
    /// Middleware responsible for attaching or generating a Correlation ID
    /// for each incoming HTTP request.
    /// This ID stays consistent through the request lifetime and is returned
    /// to the client in the response headers as 'X-Correlation-ID'.
    /// </summary>
    public class CorrelationIdMiddleware
    {
        private const string HeaderKey = "X-Correlation-ID";
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdMiddleware> _logger;

        public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Try to get CorrelationId from the incoming headers
            string correlationId = context.Request.Headers.ContainsKey(HeaderKey)
                ? context.Request.Headers[HeaderKey].ToString()
                : Guid.NewGuid().ToString();

            // Store it in HttpContext for later use (e.g., Logging, Exception Handling)
            context.Items["CorrelationId"] = correlationId;

            // Add the ID to response headers if not already present
            if (!context.Response.Headers.ContainsKey(HeaderKey))
                context.Response.Headers.Add(HeaderKey, correlationId);

            using (_logger.BeginScope("{CorrelationId}", correlationId))
            {
                _logger.LogDebug("Request started with Correlation ID: {CorrelationId}", correlationId);
                await _next(context);
                _logger.LogDebug("Request completed with Correlation ID: {CorrelationId}", correlationId);
            }
        }
    }
}
