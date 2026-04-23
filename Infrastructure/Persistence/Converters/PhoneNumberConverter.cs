using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters
{
    public class PhoneNumberConverter : ValueConverter<PhoneNumber, string>
    {
        public PhoneNumberConverter()
            : base(
                v => v.Value,
                v => new PhoneNumber(v))
        {

        }
    }
}