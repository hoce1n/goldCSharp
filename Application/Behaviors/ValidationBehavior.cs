using AppValidationException = Application.Common.Exceptions.ValidationException;
using FluentValidation;
using MediatR;

namespace Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var failures = await Task
                    .WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)))
                    .ContinueWith(t => t.Result
                        .SelectMany(r => r.Errors)
                        .Where(f => f != null)
                        .ToList(), cancellationToken);

                if (failures.Count != 0)
                {
                    var errors = failures
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(x => x.ErrorMessage).ToArray()
                        );

                    throw new AppValidationException(errors);
                }
            }

            return await next();
        }
    }
}
