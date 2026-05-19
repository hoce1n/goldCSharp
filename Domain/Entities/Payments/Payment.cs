using Domain.Common;
using Domain.Enums.Payment;
using Domain.Exceptions.Payment;

namespace Domain.Entities.Payments
{
    public class Payment : BaseEntity
    {
        public Guid UserId { get; private set; }
        public decimal Amount { get; private set; }
        public Guid? QuoteId { get; private set; }
        public PaymentStatus Status { get; private set; }

        public string Gateway { get; private set; }
        public string Authority { get; private set; } = null!;
        public string? RefId { get; private set; }

        private Payment() { }

        private Payment(
            Guid userId, 
            decimal amount, 
            Guid? quoteId,
            string gateway)
        {
            UserId = userId;
            Amount = amount;
            QuoteId = quoteId;
            Gateway = gateway;
            Status = PaymentStatus.Pending;
        }

        public static Payment Create(
            Guid userId, 
            decimal amount, 
            Guid quoteId, 
            string gateway)
        {
            if (amount <= 0)
                throw new InvalidPaymentAmountException();

            return new Payment(userId, amount, quoteId, gateway);
        }

        public void SetAuthority(string authority)
        {
            Authority = authority;
        }

        public void MarkSucceeded(string refId)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidPaymentStateException();

            Status = PaymentStatus.Succeeded;
            RefId = refId;
        }

        public void MarkFailed()
        {
            if (Status != PaymentStatus.Pending)
                return;

            Status = PaymentStatus.Failed;
        }

        public bool IsSucceeded()
        {
            return Status == PaymentStatus.Succeeded;
        }

        public bool IsPending()
        {
            return Status == PaymentStatus.Pending;
        }

    }
}
