using Domain.Enums.Karat;

namespace API.Contracts.Coin
{
    public record UpdateCoinRequest(
        string Name,
        int WeightInSoot,
        KaratType Karat,
        string? ImageUrl,
        string? Description
    );
}
