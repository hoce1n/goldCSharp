using Domain.Common.Errors;

namespace Domain.Exceptions.Order
{
    public class OrderCannotBeFailed : DomainException
    {
        public OrderCannotBeFailed()
            : base(ErrorCodes.Order.OrderCannotBeFailed, "سفارش نمیتواند شرایط لغو شدن را ندارد.")
        { }
    }
}
