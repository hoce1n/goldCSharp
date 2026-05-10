using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Common.Errors;

namespace Application.Features.Catalog.Coins.Commands.UpdateCoin
{
    public class UpdateCoinCommandHandler
        : ICommandHandler<UpdateCoinCommand, Result<UpdateCoinResponse>>
    {
        private readonly ICoinRepository _coinRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCoinCommandHandler(
            ICoinRepository coinRepository,
            IUnitOfWork unitOfWork)
        {
            _coinRepository = coinRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UpdateCoinResponse>> Handle(
            UpdateCoinCommand request,
            CancellationToken cancellationToken)
        {
            var coin = await _coinRepository.GetByIdAsync(request.Id);

            if (coin is null)
                return Result<UpdateCoinResponse>.Failure(
                    Error.Failure(ErrorCodes.Coin.NotFound, "سکه مورد نظر پیدا نشد."));
            if (coin.Name != request.Name)
            {
                var duplicatedName = await _coinRepository.ExistsByNameAsync(
                request.Name, 
                cancellationToken);

                if (duplicatedName)
                    return Result<UpdateCoinResponse>.Failure(
                        Error.Failure(ErrorCodes.Coin.DuplicateName, "یک سکه با این نام قبلا ثبت شده است."));
            }

            coin.UpdateDetails(
                request.Name,
                request.WeightInSoot,
                request.Karat,
                request.ImageUrl,
                request.Description);

            await _coinRepository.UpdateAsync(coin);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var response = new UpdateCoinResponse(
                coin.Id,
                coin.Name,
                coin.WeightInSoot,
                (int)coin.Karat,
                coin.ImageUrl,
                coin.Description
            );

            return response;
        }
    }
}
