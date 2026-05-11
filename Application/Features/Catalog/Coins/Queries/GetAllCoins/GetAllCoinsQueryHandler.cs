using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Common.Result;
using Domain.Enums.Catalog;
using Domain.Enums.Quote;

namespace Application.Features.Catalog.Coins.Queries.GetAllCoins
{
    public sealed class GetAllCoinsQueryHandler
        : IQueryHandler<GetAllCoinsQuery, Result<GetAllCoinsResponse>>
    {
        private readonly ICoinRepository _coinRepository;
        private readonly IPricingService _pricingService;
        private readonly IMarketPriceService _marketPriceService;
        public GetAllCoinsQueryHandler(
            ICoinRepository coinRepository,
            IPricingService pricingService,
            IMarketPriceService marketPriceService)
        {
            _coinRepository = coinRepository;
            _pricingService = pricingService;
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
                var (unitPrice, _) = await _pricingService.GetPriceAsync(
                    coin.Id,
                    ProductType.Coin,
                    QuoteSide.Buy,
                    cancellationToken);

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
                    unitPrice,
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
