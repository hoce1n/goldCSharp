using Microsoft.AspNetCore.Mvc;

namespace API.Extensions
{
    public static class ApiBehaviorExtensions
    {
        public static IServiceCollection AddCustomApiBehavior(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;

                options.InvalidModelStateResponseFactory = context =>
                {
                    var httpContext = context.HttpContext;

                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "One or more validation errors occurred.",
                        Detail = "The request inputs did not pass validation.",
                        Instance = httpContext.Request.Path
                    };

                    problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

                    if (httpContext.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
                        problemDetails.Extensions["correlationId"] = correlationId.ToString();

                    var errors = context.ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .Select(x => new
                        {
                            field = x.Key,
                            messages = x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        });

                    problemDetails.Extensions["errors"] = errors;

                    return new BadRequestObjectResult(problemDetails);
                };
            });

            return services;
        }
    }

}
