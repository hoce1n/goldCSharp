using Application.Abstractions.Messaging;
using Application.Common.Result;

namespace Application.Features.Quotes.Commands.ConfirmQuote
{
    public sealed record ConfirmQuoteCommand(
        Guid QuoteId,
        Guid UserId
    ) : ICommand<Result<ConfirmQuoteResponse>>;
}
