using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Common.Result;
using Application.Services.Catalog;
using Domain.Common.Errors;

namespace Application.Features.Catalog.Coins.Queries.GetCoinById
{
    public sealed class GetCoinByIdQueryHandler
        : IQueryHandler<GetCoinByIdQuery, Result<GetCoinByIdResponse>>
    {
        private readonly ICoinRepository _coinRepository;
        private readonly CoinPriceService _coinPriceService;
        private readonly MarketPriceService _marketPriceService;

        public GetCoinByIdQueryHandler(
            ICoinRepository coinRepository, 
            CoinPriceService coinPriceService,
            MarketPriceService marketPriceService)
        {
            _coinRepository = coinRepository;
            _coinPriceService = coinPriceService;
            _marketPriceService = marketPriceService;
        }

        public async Task<Result<GetCoinByIdResponse>> Handle(
            GetCoinByIdQuery request,
            CancellationToken cancellationToken)
        {
            var coin = await _coinRepository.GetByIdAsync(request.Id, cancellationToken);

            if (coin is null)
                return Result<GetCoinByIdResponse>.Failure(
                    Error.Failure(ErrorCodes.Coin.NotFound, "متاسفانه نتوانستیم محصولی که دنبال آن هستید را پیدا کنیم."));

            var finalPrice = await _coinPriceService.CalculateCoinPriceAsync(coin, cancellationToken);
            var latestSnapshot = await _marketPriceService.GetLatestSnapshotAsync(cancellationToken);

            var response = new GetCoinByIdResponse(
                coin.Id,
                coin.Name,
                coin.WeightInSoot,
                coin.Karat,
                coin.MintingFee,
                coin.Stock,
                coin.IsActive,
                coin.ImageUrl,
                coin.Description,
                coin.CreatedAt,
                finalPrice,
                latestSnapshot.CreatedAt
            );

            return response;
        }
    }
}
