namespace Application.Features.Quotes.Commands.CreateQuote
{
    public class CreateQuoteResponse
    {
        public Guid Id { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
    }
}
