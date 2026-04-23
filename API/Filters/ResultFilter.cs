using Application.Common.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters
{
    public class ResultFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult objectResult)
            {
                if (objectResult.Value is Result result)
                {
                    context.Result = HandleResult(result);
                }
                else if (objectResult.Value?.GetType().IsGenericType == true &&
                         objectResult.Value.GetType().GetGenericTypeDefinition() == typeof(Result))
                {
                    context.Result = HandleGenericResult(objectResult.Value);
                }

            }
                await next();
        }

        private IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
                return new OkResult();

            return MapError(result.Error);
        }

        private IActionResult HandleGenericResult(object resultObj)
        {
            dynamic result = resultObj;

            if (result.IsSuccess)
                return new OkObjectResult(result.Value);

            return MapError(result.Error);
        }

        private IActionResult MapError(Error error)
        {
            if (error.Code.StartsWith("not_found"))
                return new NotFoundObjectResult(error);

            if (error.Code.StartsWith("validation"))
                return new BadRequestObjectResult(error);

            if (error.Code.StartsWith("conflict"))
                return new ConflictObjectResult(error);

            return new BadRequestObjectResult(error);
        }
    }
}
