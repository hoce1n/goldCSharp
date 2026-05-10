namespace Application.Features.Catalog.Coins.Commands.UpdateStock
{
    public record UpdateCoinStockResponse(
        Guid Id,
        int Stock
    );
}
