using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Common.Errors;

namespace Application.Features.Catalog.Coins.Commands.ActivateCoin
{
    public class ActivateCoinCommandHandler
        : ICommandHandler<ActivateCoinCommand, Result<ActivateCoinResponse>>
    {
        private readonly ICoinRepository _coinRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ActivateCoinCommandHandler(
            ICoinRepository coinRepository, 
            IUnitOfWork unitOfWork)
        {
            _coinRepository = coinRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ActivateCoinResponse>> Handle(
            ActivateCoinCommand request,
            CancellationToken cancellationToken)
        {
            var coin = await _coinRepository.GetByIdAsync(request.Id);

                
            if (coin is null)
                return Result<ActivateCoinResponse>.Failure(
                    Error.Failure(ErrorCodes.Coin.NotFound, "محصول مورد نظر پیدا نشد."));

            coin.Activate();

            await _coinRepository.UpdateAsync(coin);
            await _unitOfWork.SaveChangeAsync();

            var result = new ActivateCoinResponse(
                coin.Id,
                coin.IsActive
            );

            return result;

        }
    }
}
