using Domain.Common.Errors;

namespace Application.Common.Exceptions
{
    public sealed class NotFoundException : Exception
    {
        public string Code => ErrorCodes.General.NotFound;
        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" with key \"{key}\" was not found.")
        {
        }
    }

}
