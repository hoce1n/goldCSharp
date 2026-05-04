using Application.Abstractions.Messaging;
using Application.Common.Result;
using Domain.Enums.Karat;

namespace Application.Features.Catalog.Coins.Commands.CreateCoin
{
    public sealed record CreateCoinCommand(
        string Name,
        int WeightInSoot,
        KaratType Karat,
        decimal MintingFee,
        int Stock,
        string Description,
        string ImageUrl
    ) : ICommand<Result<CreateCoinResponse>>;
}