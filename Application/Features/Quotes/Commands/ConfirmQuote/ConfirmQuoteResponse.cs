namespace Application.Features.Quotes.Commands.ConfirmQuote
{
    public class ConfirmQuoteResponse
    {
        public Guid TradeId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

}
