namespace Application.Common.Result
{
    public sealed class Error
    {
        public string Code { get; }
        public string Message { get; }

        public static readonly Error None = new Error("none", string.Empty);

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public static Error NotFound(string entityName)
            => new($"not_found.{entityName.ToLower()}", $"{entityName} not found.");

        public static Error Validation(string field, string message)
            => new($"validation.{field.ToLower()}", message);

        public static Error Conflict(string conflictName)
            => new($"conflict.{conflictName.ToLower()}", $"{conflictName} conflict occurred.");

        public static Error Unauthorized()
            => new("unauthorized", "User is not authenticated");

        public static Error Forbidden()
            => new("forbidden", "Access denied.");

        public static Error Unexpected(string message)
            => new("unexpected", message);

        public static Error AlreadyExists(string entityName)
            => new($"already_exists.{entityName.ToLower()}",
                   $"{entityName} already exists.");

        public static Error Failure(string code, string message)
        {
            return new Error(code, message);
        }


        public override string ToString() => $"{Code}: {Message}";

    }
}
