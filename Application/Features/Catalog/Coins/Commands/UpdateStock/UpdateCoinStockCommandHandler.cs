using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Common.Errors;

namespace Application.Features.Catalog.Coins.Commands.UpdateStock
{
    public class UpdateCoinStockCommandHandler
        : ICommandHandler<UpdateCoinStockCommand, Result<UpdateCoinStockResponse>>
    {
        private readonly ICoinRepository _coinRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCoinStockCommandHandler(
            ICoinRepository coinRepository, 
            IUnitOfWork unitOfWork)
        {
            _coinRepository = coinRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UpdateCoinStockResponse>> Handle(
            UpdateCoinStockCommand request,
            CancellationToken cancellationToken)
        {
            var coin = await _coinRepository.GetByIdAsync(request.Id);

            if (coin is null)
                return Result<UpdateCoinStockResponse>.Failure(
                    Error.Failure(ErrorCodes.Coin.NotFound, "محصول مورد نظر پیدا نشد."));

            coin.UpdateStock(request.Stock);

            await _coinRepository.UpdateAsync(coin);
            await _unitOfWork.SaveChangeAsync();

            var response = new UpdateCoinStockResponse(
                coin.Id,
                coin.Stock
            );

            return response;
        }
    }
}
