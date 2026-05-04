using Application.Abstractions.Messaging;
using Application.Common.Result;

namespace Application.Features.Catalog.Coins.Queries.GetAllCoins
{
    public sealed record GetAllCoinsQuery(
        bool? IsActive = null,
        int PageNumber = 1,
        int PageSize = 10
    ) : IQuery<Result<GetAllCoinsResponse>>;
}
