using Domain.Common.Errors;

namespace Domain.Exceptions.Coin
{
    public class InvalidCoinNameException : DomainException
    {
        public InvalidCoinNameException() : base(ErrorCodes.Coin.InvalidCoinName, "فرمت نام سکه صحیح نیست.") { }
    }
}
