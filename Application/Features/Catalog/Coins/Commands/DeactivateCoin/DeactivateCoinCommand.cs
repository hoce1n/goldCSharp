using Application.Abstractions.Messaging;
using Application.Common.Result;

namespace Application.Features.Catalog.Coins.Commands.DeactivateCoin
{
    public record DeactivateCoinCommand(Guid Id)
    : ICommand<Result<DeactivateCoinResponse>>;

}
