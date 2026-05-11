using Application.Abstractions.Messaging;
using Application.Common.Result;
using Domain.Enums.Catalog;
using Domain.Enums.Quote;

namespace Application.Features.Quotes.Commands.CreateQuote
{
    public sealed record CreateQuoteCommand(
        ProductType ProductType,
        decimal Amount,
        QuoteSide Side,
        Guid UserId,
        Guid? ProductId) : ICommand<Result<CreateQuoteResponse>>;
}
