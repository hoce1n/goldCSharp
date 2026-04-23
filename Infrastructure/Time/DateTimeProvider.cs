using Application.Common.Interfaces;

namespace Infrastructure.Time
{
    internal class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
