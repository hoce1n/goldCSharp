using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Common.Errors;
namespace Application.Features.Catalog.Coins.Commands.DeactivateCoin
{
    public class DeactivateCoinCommandHandler
    : ICommandHandler<DeactivateCoinCommand, Result<DeactivateCoinResponse>>
    {
        private readonly ICoinRepository _coinRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateCoinCommandHandler(
            ICoinRepository coinRepository,
            IUnitOfWork unitOfWork)
        {
            _coinRepository = coinRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<DeactivateCoinResponse>> Handle(
            DeactivateCoinCommand request,
            CancellationToken cancellationToken)
        {
            var coin = await _coinRepository.GetByIdAsync(request.Id, cancellationToken);

            if (coin is null)
                return Result<DeactivateCoinResponse>.Failure(
                    Error.Failure(ErrorCodes.Coin.NotFound, "محصول مورد نظر یافت نشد."));

            coin.Deactivate();

            await _coinRepository.UpdateAsync(coin);
            await _unitOfWork.SaveChangeAsync();

            var result = new DeactivateCoinResponse(coin.Id, coin.IsActive);
            return result;
        }
    }

}
