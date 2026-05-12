using Domain.Common.Errors;

namespace Domain.Exceptions.Order
{
    public class OnlyActiveOrdersCanBeCanceled : DomainException
    {
        public OnlyActiveOrdersCanBeCanceled()
            : base(ErrorCodes.Order.OnlyActiveOrdersCanBeCanceled, "فقط سفارش های فعال میتوانند کنسل شوند.")
        {}
    }
}
