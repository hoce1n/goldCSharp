using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Common.Result;
using Application.Services.Catalog;
using Domain.Common.Errors;

namespace Application.Features.Catalog.MarketPrices.Queries.GetLatest
{
    public sealed class GetLatestMarketPriceQueryHandler
        : IQueryHandler<GetLatestMarketPriceQuery, Result<GetLatestMarketPriceResponse>>
    {
        private readonly MarketPriceService _marketPriceService;

        public GetLatestMarketPriceQueryHandler(MarketPriceService marketPriceService)
        {
            _marketPriceService = marketPriceService;
        }

        public async Task<Result<GetLatestMarketPriceResponse>> Handle(
            GetLatestMarketPriceQuery request,
            CancellationToken cancellationToken)
        {
            var latestSnapshot = await _marketPriceService.GetLatestSnapshotAsync(cancellationToken);

            if (latestSnapshot is null)
                return Result<GetLatestMarketPriceResponse>.Failure(
                    Error.Failure(ErrorCodes.General.NotFound, "قیمت ذخیره نشده."));

            var response = new GetLatestMarketPriceResponse
            {
                PricePerGram = latestSnapshot.Price18PerGram,
                PriceTimestamp = latestSnapshot.CreatedAt
            };

            return response;
        }
    }
}
