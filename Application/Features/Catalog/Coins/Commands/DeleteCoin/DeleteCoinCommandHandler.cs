using Application.Abstractions.Repositories;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Common.Errors;
using MediatR;

namespace Application.Features.Catalog.Coins.Commands.DeleteCoin
{
    public class DeleteCoinCommandHandler
    : IRequestHandler<DeleteCoinCommand, Result>
    {
        private readonly ICoinRepository _coinRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCoinCommandHandler(
            ICoinRepository coinRepository,
            IUnitOfWork unitOfWork)
        {
            _coinRepository = coinRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            DeleteCoinCommand request,
            CancellationToken cancellationToken)
        {
            var coin = await _coinRepository.GetByIdAsync(request.Id, cancellationToken);

            if (coin is null)
                return Result.Failure(
                    Error.Failure(ErrorCodes.Coin.NotFound, "محصول مورد نظر یافت نشد."));

            await _coinRepository.DeleteAsync(coin);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return Result.Success();
        }
    }

}
