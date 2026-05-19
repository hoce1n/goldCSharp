namespace Application.Abstractions.Payments
{
    public class PaymentVerifyResult
    {
        public bool IsSuccessful { get; set; }
        public string? RefId { get; set; }
        public string? Message { get; set; }
    }

}
