namespace Application.Abstractions.Payments
{
    public interface IPaymentGateway
    {
        Task<PaymentGatewayResult> CreatePayment(
            Guid paymentId,
            decimal amount,
            CancellationToken cancellationToken);

        Task<PaymentVerifyResult> VerifyPayment(
            string authority,
            decimal amount,
            CancellationToken cancellationToken);
    }
}
