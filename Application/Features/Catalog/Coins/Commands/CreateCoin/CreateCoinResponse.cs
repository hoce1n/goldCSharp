using Domain.Enums.Karat;

namespace Application.Features.Catalog.Coins.Commands.CreateCoin
{
    public sealed record CreateCoinResponse(
        Guid Id,
        string Name,
        int WeightInSoot,
        KaratType Karat,
        decimal MintingFee,
        int Stock,
        bool IsActive
    );
}