using Domain.Common.Errors;

namespace Domain.Exceptions.Wallet
{
    public class InsufficientBalanceException : DomainException
    {
        public InsufficientBalanceException()
            : base(ErrorCodes.Wallet.InsufficientBalance, "موجودی ناکافی ست.")
        { }
    }
}
