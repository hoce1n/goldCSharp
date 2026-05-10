using Application.Abstractions.Messaging;
using Application.Common.Result;

namespace Application.Features.Catalog.Coins.Commands.ActivateCoin
{
    public record ActivateCoinCommand(Guid Id)
        : ICommand<Result<ActivateCoinResponse>>;
}
