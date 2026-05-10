namespace Application.Features.Catalog.Coins.Commands.UpdateMintingFee
{
    public record UpdateCoinMintingFeeResponse(
        Guid Id,
        decimal MintingFee
    );
}
