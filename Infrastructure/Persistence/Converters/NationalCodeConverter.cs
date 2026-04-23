using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters
{
    public class NationalCodeConverter : ValueConverter<NationalCode, string>
    {
        public NationalCodeConverter()
            : base(
                v => v.Value,
                v => new NationalCode(v))
        {

        }
    }
}