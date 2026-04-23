using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters
{
    public class EmailConverter : ValueConverter<Email, string>
    {
        public EmailConverter()
            : base(
                v => v.Value,
                v => new Email(v))
        {

        }
    }
}