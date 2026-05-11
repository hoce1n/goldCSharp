using Domain.Common.Errors;

namespace Domain.Exceptions.Quote
{
    public class InvalidQuoteAmountException : DomainException
    {
        public InvalidQuoteAmountException()
            : base(ErrorCodes.Quote.InvalidAmount, "درخواست نامعتبر است.")
        { }
    }
}
