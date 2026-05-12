using Domain.Common.Errors;

namespace Domain.Exceptions.Order
{
    public class CanNotBeCompleted : DomainException
    {
        public CanNotBeCompleted()
             : base(ErrorCodes.Order.CanNotBeCompleted, "سفارش نمیتواند نهایی شود.")
        { }
    }
}
