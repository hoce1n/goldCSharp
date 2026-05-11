using Domain.Common.Errors;

namespace Domain.Exceptions.Quote
{
    public class QuoteAlreadyConfirmedException : DomainException
    {
        public QuoteAlreadyConfirmedException()
            : base(ErrorCodes.Quote.QuoteAlreadyConfirmed, "قبلا تایید شده است.")
            {}
    }
}
