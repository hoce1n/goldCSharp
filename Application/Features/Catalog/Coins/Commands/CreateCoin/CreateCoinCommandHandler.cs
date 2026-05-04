using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Common.Errors;
using Domain.Entities.Catalog;
using Mapster;
using MediatR;

namespace Application.Features.Catalog.Coins.Commands.CreateCoin
{
    public sealed class CreateCoinCommandHandler
        : ICommandHandler<CreateCoinCommand, Result<CreateCoinResponse>>
    {
        private readonly ICoinRepository _coinRepository;
        private readonly IUnitOfWork _unitOfWork;
        
        public CreateCoinCommandHandler(
            ICoinRepository coinRepository, 
            IUnitOfWork unitOfWork )
        {
            _coinRepository = coinRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreateCoinResponse>> Handle(
            CreateCoinCommand command,
            CancellationToken cancellationToken)
        {
            var existingCoin = await _coinRepository
                .GetByNameAsync(command.Name, cancellationToken);

            if (existingCoin is not null)
                return Result<CreateCoinResponse>.Failure(
                    Error.Failure(ErrorCodes.Coin.DuplicateName, "محصولی با این نام قبلا ثبت شده است."));

            var coin = Coin.Create(
                name: command.Name,
                weightInSoot: command.WeightInSoot,
                karat: command.Karat,
                mintingFee: command.MintingFee,
                initialStock: command.Stock,
                description: command.Description,
                imageUrl: command.ImageUrl
            );

            await _coinRepository.AddAsync(coin);
            await _unitOfWork.SaveChangeAsync();

            var response = coin.Adapt<CreateCoinResponse>();
            return response;
        }
    }
}
