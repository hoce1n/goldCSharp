using Domain.Common;
using Domain.Enums.Catalog;
using Domain.Enums.Quote;
using Domain.Exceptions.Quote;

namespace Domain.Entities.Quote
{
    public class Quote : BaseEntity
    {
        public Guid UserId { get; private set; }
        public ProductType ProductType { get; private set; }
        public Guid ProductId { get; private set; }
        public QuoteSide Side { get; private set; }
        public decimal RequestAmount { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal TotalPrice { get; private set; }
        public Guid PriceSnapshotId { get; private set; }
        public DateTime ExpiredAtUtc { get; private set; }
        public QuoteStatus Status { get; private set; }

        private Quote() { }

        public static Quote Create(
            Guid userId,
            ProductType productType,
            Guid productId,
            QuoteSide side,
            decimal requestAmount,
            decimal unitPrice,
            Guid priceSnapshotId,
            int expiresInSeconds)
        {
            if (requestAmount <= 0)
                throw new InvalidQuoteAmountException();

            return new Quote
            {
                ProductType = productType,
                UserId = userId,
                ProductId = productId,
                Side = side,
                RequestAmount = requestAmount,
                UnitPrice = unitPrice,
                TotalPrice = requestAmount * unitPrice,
                PriceSnapshotId = priceSnapshotId,
                ExpiredAtUtc = DateTime.UtcNow.AddSeconds(expiresInSeconds),
                Status = QuoteStatus.Active
            };
        }

        public void Confirm(DateTime now)
        {
            if (Status != QuoteStatus.Active)
                throw new QuoteIsNotActiveException();

            if (now > ExpiredAtUtc)
                throw new QuoteExpiredExceptions();

            Status = QuoteStatus.Confirmed;
            SetUpdated();
        }

        public void Expire(DateTime now)
        {
            if (Status != QuoteStatus.Active)
                return;

            Status = QuoteStatus.Expired;
            SetUpdated();
        }

        public bool IsExpired(DateTime now)
        {
            return now > ExpiredAtUtc;
        }
    }

}
