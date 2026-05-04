using Domain.Common.Errors;
using Domain.Exceptions;

namespace Domain.Excetions.Coin
{
    public class InvalidWeightException : DomainException
    {
        public InvalidWeightException()
            : base(ErrorCodes.Coin.InvalidWeight, "وزن سکه باید مثبت باشد.") { }
    }
}
