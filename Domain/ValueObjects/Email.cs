using System.Text.RegularExpressions;
using Domain.Common;
using Domain.Exceptions.User;

namespace Domain.ValueObjects
{
    public sealed class Email : ValueObject
    {
        public string Value { get; private set; }

        private Email() { }
        
        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidEmailException();

            value = value.Trim().ToLower();

            if (!IsValidEmail(value))
                throw new InvalidEmailException();

            Value = value;
        }

        private static bool IsValidEmail(string email)
        {
            const string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, pattern);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}