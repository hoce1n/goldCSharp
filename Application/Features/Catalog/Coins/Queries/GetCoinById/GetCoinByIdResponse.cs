using Domain.Enums.Karat;

namespace Application.Features.Catalog.Coins.Queries.GetCoinById
{
    public record GetCoinByIdResponse
    (
        Guid Id,
        string Name,
        int WeightInSoot,
        KaratType Karat,
        decimal MintingFee,
        int Stock,
        bool IsActive,
        string? ImageUrl,
        string? Description,
        DateTime CreatedAt,

        long FinalPrice,
        DateTime PriceSnapshotTimeStamp
    );

}
