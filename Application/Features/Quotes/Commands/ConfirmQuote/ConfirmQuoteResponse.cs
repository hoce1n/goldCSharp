namespace Application.Features.Quotes.Commands.ConfirmQuote
{
    public class ConfirmQuoteResponse
    {
        public Guid OrderId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? PaymentUrl { get; set; }
        public bool RequiresPayment { get; set; }
    }

}
