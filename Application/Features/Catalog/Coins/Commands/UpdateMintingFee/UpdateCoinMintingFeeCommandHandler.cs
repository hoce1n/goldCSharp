using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Common.Errors;

namespace Application.Features.Catalog.Coins.Commands.UpdateMintingFee
{
    public class UpdateCoinMintingFeeCommandHandler
        : ICommandHandler<UpdateCoinMintingFeeCommand, Result<UpdateCoinMintingFeeResponse>>
    {
        private readonly ICoinRepository _coinRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCoinMintingFeeCommandHandler(
            ICoinRepository coinRepository, 
            IUnitOfWork unitOfWork)
        {
            _coinRepository = coinRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UpdateCoinMintingFeeResponse>> Handle(
            UpdateCoinMintingFeeCommand request,
            CancellationToken cancellationToken)
        {
            var coin = await _coinRepository.GetByIdAsync(request.Id);

            if (coin is null)
                return Result<UpdateCoinMintingFeeResponse>.Failure(
                    Error.Failure(ErrorCodes.Coin.NotFound, "محصول مورد نظر پیدا نشد."));

            coin.SetMintingFee(request.MintingFee);

            await _coinRepository.UpdateAsync(coin);
            await _unitOfWork.SaveChangeAsync();

            var response = new UpdateCoinMintingFeeResponse(
                coin.Id,
                coin.MintingFee
            );

            return response;
        }
    }
}
