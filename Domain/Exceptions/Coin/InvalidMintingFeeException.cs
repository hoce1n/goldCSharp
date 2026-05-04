using Domain.Common.Errors;

namespace Domain.Exceptions.Coin
{
    public class InvalidMintingFeeException : DomainException
    {
        public InvalidMintingFeeException()
            : base(ErrorCodes.Coin.InvalidMintingFee, "حق ضرب نمی‌تواند منفی باشد.") { }
    }

}
