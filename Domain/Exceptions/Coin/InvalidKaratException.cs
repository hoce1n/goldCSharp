using Domain.Common.Errors;

namespace Domain.Exceptions.Coin
{
    public class InvalidKaratException : DomainException
    {
        public InvalidKaratException()
            : base(ErrorCodes.Coin.InvalidKarat, "عیار باید بین 0 تا 1 باشد.") { }
    }

}
