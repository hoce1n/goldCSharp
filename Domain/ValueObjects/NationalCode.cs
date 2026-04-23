using Domain.Common;
using Domain.Exceptions.User;

namespace Domain.ValueObjects
{
    public class NationalCode : ValueObject
    {
        public string Value { get; }
        public NationalCode(string value)
        {
            if(!IsValid(value))
                throw new InvalidNationalCodeException();
        }

        public static bool IsValid(string code)
        {
            if (string.IsNullOrWhiteSpace(code) || code.Length != 10)
                return false;

            var check = int.Parse(code.Substring(9, 1));
            var sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(code[i].ToString()) * (10 - i);

            var remainder = sum % 11;
            return (remainder < 2 && check == remainder) || (remainder >= 2 && check == 11 - remainder);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}