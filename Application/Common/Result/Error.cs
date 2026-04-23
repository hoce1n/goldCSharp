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
        public override string ToString() => $"{Code}: {Message}";

    }
}
