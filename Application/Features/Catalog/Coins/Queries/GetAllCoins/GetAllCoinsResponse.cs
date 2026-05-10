using Domain.Enums.Karat;

namespace Application.Features.Catalog.Coins.Queries.GetAllCoins
{
    public sealed record GetAllCoinsResponse(
        List<CoinDto> Coins,
        int TotalCount,
        int PageNumber,
        int PageSize
        
    );
     
    public sealed record CoinDto(
        Guid Id,
        string Name,
        int WeightInSoot,
        KaratType Karat,
        decimal MintingFee,
        int Stock,
        string? ImageUrl,
        string? Description,
        bool IsActive,
        DateTime CreatedAt,
        long FinalPrice,
        DateTime PriceSnapshotTimeStamp
    );
}
