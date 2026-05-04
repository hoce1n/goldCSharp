using Application.Abstractions.Messaging;
using Application.Common.Result;

namespace Application.Features.Catalog.Coins.Queries.GetCoinById
{
    public record GetCoinByIdQuery(Guid Id) 
        : IQuery<Result<GetCoinByIdResponse>>;
}
