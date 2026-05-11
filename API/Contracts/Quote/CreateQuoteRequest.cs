using Domain.Enums.Catalog;
using Domain.Enums.Quote;

namespace API.Contracts.Quote
{
    public class CreateQuoteRequest
    {
        public ProductType ProductType { get; set; }
        public decimal Amount { get; set; }
        public QuoteSide Side { get; set; }
        public Guid? ProductId { get; set; } 
    }
}
