using Domain.Common.Errors;
namespace Domain.Exceptions.Payment
{
    public class InvalidPaymentAmountException : DomainException
    {
        public InvalidPaymentAmountException()
            : base(ErrorCodes.Payment.InvalidPaymentAmount, "Invalid_Payment_Amount")
        { }
    }
}