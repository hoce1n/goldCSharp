using Application.Abstractions.Messaging;
using Application.Common.Result;
using Domain.Enums.Karat;

namespace Application.Features.Catalog.Coins.Commands.UpdateCoin
{
    public record UpdateCoinCommand(
        Guid Id, 
        string Name,
        int WeightInSoot,
        KaratType Karat,
        string? ImageUrl,
        string? Description
    ) : ICommand<Result<UpdateCoinResponse>>;
}
