
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Common.Errors;
using Domain.Enums.Catalog;

namespace Application.Features.Quotes.Commands.ConfirmQuote
{
    public sealed class ConfirmQuoteCommandHandler
        : ICommandHandler<ConfirmQuoteCommand, Result<ConfirmQuoteResponse>>
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICoinRepository _coinRepository;

        public ConfirmQuoteCommandHandler(
            IQuoteRepository quoteRepository, 
            ICoinRepository coinRepository,
            IUnitOfWork unitOfWork)
        {
            _quoteRepository = quoteRepository;
            _coinRepository = coinRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ConfirmQuoteResponse>> Handle(
            ConfirmQuoteCommand command,
            CancellationToken cancellationToken)
        {
            var quote = await _quoteRepository.GetByIdAsync(command.QuoteId);

            if (quote is null)
                return Result<ConfirmQuoteResponse>.Failure(
                    Error.Failure(ErrorCodes.Quote.NotFound, "مظنه یافت نشد."));

            if (quote.UserId != command.UserId)
                return Result<ConfirmQuoteResponse>.Failure(
                    Error.Failure(ErrorCodes.Auth.Unauthorized, "دسترسی غیرمجاز"));

            if (quote.Status != Domain.Enums.Quote.QuoteStatus.Active)
                return Result<ConfirmQuoteResponse>.Failure(
                    Error.Failure(ErrorCodes.Quote.IsNotActive, "مظنه فعال نیست."));

            if (DateTime.UtcNow > quote.ExpiredAtUtc)
            {
                quote.Expire();
                await _unitOfWork.SaveChangeAsync();
                return Result<ConfirmQuoteResponse>.Failure(
                    Error.Failure(ErrorCodes.Quote.Expired, "مظنه منقضی شده."));
            }

            if (quote.ProductType == ProductType.Coin)
            {
                var coin = await _coinRepository.GetByIdAsync(quote.ProductId, cancellationToken);
                if (coin is null)
                    return Result<ConfirmQuoteResponse>.Failure(
                        Error.Failure(ErrorCodes.Coin.NotFound, "سکه پیدا نشد."));

                if (coin.Stock < quote.Amount)
                    return Result.Failure<ConfirmQuoteResponse>("Insufficient inventory");

                coin.DecreaseStock(quote.Amount);
            }

        }
    }
}
