using Application.Abstractions.Messaging;
using Application.Common.Result;

namespace Application.Features.Catalog.Coins.Commands.UpdateStock
{
    public record UpdateCoinStockCommand(
        Guid Id,
        int Stock
    ) : ICommand<Result<UpdateCoinStockResponse>>;
}
