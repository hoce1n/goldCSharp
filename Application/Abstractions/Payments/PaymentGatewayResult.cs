namespace Application.Abstractions.Payments
{
    public class PaymentGatewayResult
    {
        public string Authority { get; set; } = null!;
        public string PaymentUrl { get; set; } = null!;
    }
}
