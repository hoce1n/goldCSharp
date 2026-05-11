using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Common.Result;
using Domain.Common.Errors;
using Domain.Enums.Catalog;
using Domain.Enums.Quote;

namespace Application.Features.Catalog.Coins.Queries.GetCoinById
{
    public sealed class GetCoinByIdQueryHandler
        : IQueryHandler<GetCoinByIdQuery, Result<GetCoinByIdResponse>>
    {
        private readonly ICoinRepository _coinRepository;
        private readonly IPricingService _pricingService;
        private readonly IMarketPriceService _marketPriceService;

        public GetCoinByIdQueryHandler(
            ICoinRepository coinRepository,
            IPricingService pricingService,
            IMarketPriceService marketPriceService)
        {
            _coinRepository = coinRepository;
            _pricingService = pricingService;
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

            var (unitPrice, _) = await _pricingService.GetPriceAsync(
                coin.Id,
                ProductType.Coin,
                QuoteSide.Buy,
                cancellationToken);

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
                unitPrice,
                latestSnapshot.CreatedAt
            );

            return response;
        }
    }
}
