using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Common.Errors;
using Domain.Entities.Order;

namespace Application.Features.Quotes.Commands.ConfirmQuote
{
    public sealed class ConfirmQuoteCommandHandler
        : ICommandHandler<ConfirmQuoteCommand, Result<ConfirmQuoteResponse>>
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmQuoteCommandHandler(
            IQuoteRepository quoteRepository, 
            IUnitOfWork unitOfWork,
            IOrderRepository orderRepository)
        {
            _quoteRepository = quoteRepository;
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
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

            quote.Confirm();

            var order = Order.Create(
                quote.UserId,
                quote.ProductId,
                quote.ProductType,
                quote.Side,
                quote.RequestAmount,
                quote.UnitPrice,
                quote.TotalPrice,
                quote.Id
            );

            await _orderRepository.AddAsync(order, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var response = new ConfirmQuoteResponse
            {
                OrderId = order.Id,
                UnitPrice = quote.UnitPrice,
                TotalPrice = quote.TotalPrice,
            };

            return response;

        }
    }
}
