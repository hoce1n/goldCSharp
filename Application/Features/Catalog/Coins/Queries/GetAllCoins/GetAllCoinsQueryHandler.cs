using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Common.Result;
using Mapster;

namespace Application.Features.Catalog.Coins.Queries.GetAllCoins
{
    public sealed class GetAllCoinsQueryHandler
        : IQueryHandler<GetAllCoinsQuery, Result<GetAllCoinsResponse>>
    {
        private readonly ICoinRepository _coinRepository;

        public GetAllCoinsQueryHandler(ICoinRepository coinRepository)
        {
            _coinRepository = coinRepository;
        }

        public async Task<Result<GetAllCoinsResponse>> Handle(
            GetAllCoinsQuery request,
            CancellationToken cancellationToken)
        {
            var (coins, totalCount) = await _coinRepository.GetPagedAsync(
                request.IsActive,
                request.PageNumber,
                request.PageSize,
                cancellationToken
            );

            var coinDtos = coins.Adapt<List<CoinDto>>();

            var response = new GetAllCoinsResponse(
                coinDtos,
                totalCount,
                request.PageNumber,
                request.PageSize
            );

            return response;
        }
    }
}
