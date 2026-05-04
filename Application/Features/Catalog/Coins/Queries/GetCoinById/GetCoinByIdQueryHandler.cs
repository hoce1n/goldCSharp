using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Common.Result;
using Domain.Common.Errors;

namespace Application.Features.Catalog.Coins.Queries.GetCoinById
{
    public sealed class GetCoinByIdQueryHandler
        : IQueryHandler<GetCoinByIdQuery, Result<GetCoinByIdResponse>>
    {
        private readonly ICoinRepository _coinRepository;

        public GetCoinByIdQueryHandler(ICoinRepository coinRepository)
        {
            _coinRepository = coinRepository;
        }

        public async Task<Result<GetCoinByIdResponse>> Handle(
            GetCoinByIdQuery request,
            CancellationToken cancellationToken)
        {
            var coin = await _coinRepository.GetByIdAsync(request.Id, cancellationToken);

            if (coin is null)
                return Result<GetCoinByIdResponse>.Failure(
                    Error.Failure(ErrorCodes.Coin.NotFound, "متاسفانه نتوانستیم محصولی که دنبال آن هستید را پیدا کنیم."));

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
                coin.CreatedAt
            );

            return response;
        }
    }
}
