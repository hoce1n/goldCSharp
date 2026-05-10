using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Common.Result;
using Application.Services.Catalog;

namespace Application.Features.Catalog.Coins.Queries.GetAllCoins
{
    public sealed class GetAllCoinsQueryHandler
        : IQueryHandler<GetAllCoinsQuery, Result<GetAllCoinsResponse>>
    {
        private readonly ICoinRepository _coinRepository;
        private readonly CoinPriceService _coinPriceService;
        private readonly MarketPriceService _marketPriceService;
        public GetAllCoinsQueryHandler(
            ICoinRepository coinRepository,
            CoinPriceService coinPriceService,
            MarketPriceService marketPriceService)
        {
            _coinRepository = coinRepository;
            _coinPriceService = coinPriceService;
            _marketPriceService = marketPriceService;
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

            var latestSnapshot = await _marketPriceService.GetLatestSnapshotAsync(cancellationToken);

            var coinDtos = new List<CoinDto>(coins.Count);

            foreach (var coin in coins)
            {
                var finalPrice = await _coinPriceService
                    .CalculateCoinPriceAsync(coin, cancellationToken);

                coinDtos.Add(new CoinDto(
                    coin.Id,
                    coin.Name,
                    coin.WeightInSoot,
                    coin.Karat,
                    coin.MintingFee,
                    coin.Stock,
                    coin.ImageUrl,
                    coin.Description,
                    coin.IsActive,
                    coin.CreatedAt,
                    finalPrice,
                    latestSnapshot.CreatedAt
                ));
            }

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
