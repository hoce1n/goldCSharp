using Domain.Common.Errors;

namespace Domain.Exceptions.Payment
{
    public class InvalidPaymentStateException : DomainException
    {
        public InvalidPaymentStateException()
            : base(ErrorCodes.Payment.InvalidPaymentState, "Invalid_Payment_State")
        { }
    }
}
