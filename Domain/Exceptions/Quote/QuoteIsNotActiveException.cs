using Domain.Common.Errors;

namespace Domain.Exceptions.Quote
{
    public class QuoteIsNotActiveException : DomainException
    {
        public QuoteIsNotActiveException()
            : base(ErrorCodes.Quote.IsNotActive, "Quote is not active")
        { }
    }
}
