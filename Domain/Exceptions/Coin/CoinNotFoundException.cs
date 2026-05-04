using Domain.Common.Errors;

namespace Domain.Exceptions.Coin
{
    public class CoinNotFoundException : DomainException
    {
        public CoinNotFoundException(Guid coinId)
            : base(ErrorCodes.Coin.NotFound, $"سکه با شناسه {coinId} یافت نشد.") { }
    }
}
