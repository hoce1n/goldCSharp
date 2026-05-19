using Domain.Common.Errors;

namespace Domain.Exceptions.Wallet
{
    internal class InvalidWalletAmountException : DomainException
    {
        public InvalidWalletAmountException()
            : base(ErrorCodes.Wallet.InvalidWalletAmount, "مقدار ورودی به کیف پول نامعتبر می‌باشد.")
        {

        }
    }
}
