using Application.Abstractions.Messaging;
using Application.Common.Result;

namespace Application.Features.Catalog.Coins.Commands.UpdateMintingFee
{
    public record UpdateCoinMintingFeeCommand(
        Guid Id,
        decimal MintingFee
    ) : ICommand<Result<UpdateCoinMintingFeeResponse>>;
}