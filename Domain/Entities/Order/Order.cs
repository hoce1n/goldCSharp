using Domain.Common;
using Domain.Enums.Catalog;
using Domain.Enums.Order;
using Domain.Enums.Quote;
using Domain.Exceptions;
using Domain.Exceptions.Order;

namespace Domain.Entities.Order
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; private set; }
        public Guid ProductId { get; private set; }
        public ProductType ProductType { get; private set; }
        public QuoteSide QuoteSide { get; private set; }
        public decimal RequestAmount { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal TotalPrice { get; private set; }
        public Guid QuoteId { get; private set; }
        public OrderStatus Status { get; private set; }
        public string IdempotencyKey { get; private set; } = null!;

        private Order() { }

        private Order(
            Guid userId, 
            Guid productId, 
            ProductType productType, 
            QuoteSide side, 
            decimal requestAmount, 
            decimal unitPrice, 
            decimal totalPrice, 
            Guid quoteId,
            string idempotencyKey)
        {
            UserId = userId;
            ProductId = productId;
            ProductType = productType;
            QuoteSide = side;
            RequestAmount = requestAmount;
            UnitPrice = unitPrice;
            TotalPrice = totalPrice;
            QuoteId = quoteId;
            IdempotencyKey = idempotencyKey;
            Status = OrderStatus.Created;
            CreatedAt = DateTime.UtcNow;
        }

        public static Order Create(
            Guid userId,
            Guid productId,
            ProductType productType,
            QuoteSide side,
            decimal requestAmount,
            decimal unitPrice,
            decimal totalPrice,
            Guid quoteId,
            string idempotencyKey)
        {

            return new Order(
                userId,
                productId,
                productType,
                side,
                requestAmount,
                unitPrice,
                totalPrice,
                quoteId,
                idempotencyKey
            );
        }

        public void MarkCompleted()
        {
            if (Status != OrderStatus.Created)
                throw new CanNotBeCompleted();

            Status = OrderStatus.Completed;
        }

        public void MarkFailed()
        {
            if (Status != OrderStatus.Created)
                throw new OrderCannotBeFailed();

            Status = OrderStatus.Failed;

        }

        public void Cancel()
        {
            if (Status != OrderStatus.Created)
                throw new OnlyActiveOrdersCanBeCanceled();

            Status = OrderStatus.Canceled;
        }
    }
}
