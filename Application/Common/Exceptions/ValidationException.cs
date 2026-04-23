using Domain.Common.Errors;

namespace Application.Common.Exceptions
{
    public sealed class ValidationException : Exception
    {
        public string Code => ErrorCodes.General.Validation;
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(IDictionary<string, string[]> errors)
            : base("One or more validation failures have occurred")
        {
            Errors = errors;
        }
    }

}
