using Application.Abstractions.Repositories;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Common.Errors;
using Domain.Entities.Quote;
using Domain.Enums.Catalog;

namespace Application.Features.Quotes.Commands.CreateQuote
{
    public class CreateQuoteCommandHandler
        : ICommandHandler<CreateQuoteCommand, Result<CreateQuoteResponse>>
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IPricingService _pricingService;
        private readonly IMeltedGoldRepository _meltedGoldRepository;
        private readonly ICoinRepository _coinRepository;
        private readonly IUnitOfWork _unitOfWork;

        private const int QuoteExpirationSeconds = 60;

        public CreateQuoteCommandHandler(
            IQuoteRepository quoteRepository,
            IMeltedGoldRepository meltedGoldRepository,
            ICoinRepository coinRepository,
            IPricingService pricingService,
            IUnitOfWork unitOfWork)
        {
            _quoteRepository = quoteRepository;
            _pricingService = pricingService;
            _meltedGoldRepository = meltedGoldRepository;
            _coinRepository = coinRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreateQuoteResponse>> Handle(
            CreateQuoteCommand command,
            CancellationToken cancellationToken)
        {
            if (command.Amount <= 0)
                return Result<CreateQuoteResponse>.Failure(
                    Error.Failure(ErrorCodes.Quote.InvalidAmount, "amount invalid"));

            Guid ProductId;
            switch (command.ProductType)
            {
                case ProductType.MeltedGold:
                    var meltedGold =
                        await _meltedGoldRepository.GetActiveOrCreateAsync(cancellationToken);
                    ProductId = meltedGold.Id;
                    break;

                case ProductType.Coin:
                    if (command.ProductId is null)
                        return Result<CreateQuoteResponse>.Failure(
                            Error.Failure(ErrorCodes.Coin.IdNotFound, "شناسه سکه الزامی ست."));

                    var coin = await _coinRepository.GetByIdAsync(command.ProductId.Value, cancellationToken);
                    if (coin is null)
                        return Result<CreateQuoteResponse>.Failure(
                            Error.Failure(ErrorCodes.Coin.NotFound, "سکه پیدا نشد."));

                    ProductId = coin.Id;
                    break;

                default:
                    return Result<CreateQuoteResponse>.Failure(
                        Error.Failure(ErrorCodes.General.NotFound, "تایپ سکه یافت نشد."));

            }

            var (unitPrice, snapshotId) =
                await _pricingService.GetPriceAsync(
                    ProductId, 
                    command.ProductType,
                    command.Side,
                    cancellationToken);

            var quote = Quote.Create(
                command.UserId,
                command.ProductType,
                ProductId,
                command.Side,
                command.Amount,
                unitPrice,
                snapshotId,
                expiresInSeconds: QuoteExpirationSeconds);

            await _quoteRepository.AddAsync(quote, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var response = new CreateQuoteResponse
            {
                Id = quote.Id,
                UnitPrice = quote.UnitPrice,
                TotalPrice = quote.TotalPrice,
                ExpiresAtUtc = quote.ExpiredAtUtc
            };

            return response;
        }
    }
}
