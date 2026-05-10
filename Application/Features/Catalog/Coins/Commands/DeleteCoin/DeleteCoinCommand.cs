using Application.Abstractions.Messaging;
using Application.Common.Result;

namespace Application.Features.Catalog.Coins.Commands.DeleteCoin
{
    public record DeleteCoinCommand(Guid Id)
    : ICommand<Result>;

}
