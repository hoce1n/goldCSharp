using Application.Common.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters
{
    public class ResultFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(
            ResultExecutingContext context, 
            ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult objectResult)
            {
                var value = objectResult.Value;

                if (value is Result nonGeneric)
                {
                    context.Result = WrapNonGeneric(nonGeneric);
                }
                else if (IsGenericResult(value))
                {
                    context.Result = WrapGeneric(value);
                }
            }
                await next();
        }

        private bool IsGenericResult(object? value)
        {
            if (value == null)
                return false;

            var type = value.GetType();
            return type.IsGenericType &&
                   type.GetGenericTypeDefinition() == typeof(Result<>);
        }

        private IActionResult WrapNonGeneric(Result result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(new
                {
                    success = true,
                    data = (object?)null,
                    error = (object?)null
                });
            }

            return MapError(result.Error);
        }

        private IActionResult WrapGeneric(object resultObj)
        {
            dynamic result = resultObj;

            if (result.IsSuccess)
            {
                return new OkObjectResult(new
                {
                    success = true,
                    data = result.Value,
                    error = (object?)null
                });
            }

            return MapError(result.Error);
        }

        private IActionResult MapError(Error error)
        {
            var statusCode = error.Code switch
            {
                var c when c.Contains("not_found", StringComparison.OrdinalIgnoreCase) => 404,
                var c when c.Contains("unauthorized", StringComparison.OrdinalIgnoreCase) => 401,
                var c when c.Contains("forbidden", StringComparison.OrdinalIgnoreCase) => 403,
                var c when c.Contains("conflict", StringComparison.OrdinalIgnoreCase) => 409,
                var c when c.Contains("invalid", StringComparison.OrdinalIgnoreCase) => 400,
                _ => 400
            };

            return new ObjectResult(new
            {
                success = false,
                data = (object?)null,
                error = new
                {
                    code = error.Code,
                    message = error.Message
                }
            })
            {
                StatusCode = statusCode
            };
        }
    }
}
