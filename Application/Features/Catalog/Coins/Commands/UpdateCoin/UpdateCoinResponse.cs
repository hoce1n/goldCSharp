namespace Application.Features.Catalog.Coins.Commands.UpdateCoin
{
    public record UpdateCoinResponse(
        Guid Id,
        string Name,
        int WeightInSoot,
        int Karat,
        string? ImageUrl,
        string? Description
    );
}
