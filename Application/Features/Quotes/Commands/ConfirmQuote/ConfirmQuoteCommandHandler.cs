using Application.Abstractions.Messaging;
using Application.Abstractions.Payments;
using Application.Abstractions.Repositories;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Common.Errors;
using Domain.Entities.Order;
using Domain.Enums.Quote;
using Domain.Entities.Payments;

namespace Application.Features.Quotes.Commands.ConfirmQuote
{
    public sealed class ConfirmQuoteCommandHandler
        : ICommandHandler<ConfirmQuoteCommand, Result<ConfirmQuoteResponse>>
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ConfirmQuoteCommandHandler(
            IQuoteRepository quoteRepository, 
            IOrderRepository orderRepository,
            IWalletRepository walletRepository,
            IPaymentRepository paymentRepository,
            IPaymentGateway paymentGateway,
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider)
        {
            _quoteRepository = quoteRepository;
            _orderRepository = orderRepository;
            _walletRepository = walletRepository;
            _paymentRepository = paymentRepository;
            _paymentGateway = paymentGateway;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<ConfirmQuoteResponse>> Handle(
            ConfirmQuoteCommand command,
            CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var existingOrder = await _orderRepository
                .GetByIdempotencyKeyAsync(command.IdempotencyKey, cancellationToken);

            if (existingOrder is not null)
            {
                return new ConfirmQuoteResponse
                {
                    OrderId = existingOrder.Id,
                    UnitPrice = existingOrder.UnitPrice,
                    TotalPrice = existingOrder.TotalPrice,
                };
            }

            var quote = await _quoteRepository.GetByIdAsync(command.QuoteId, cancellationToken);

            if (quote is null)
                return Result<ConfirmQuoteResponse>.Failure(
                    Error.Failure(ErrorCodes.Quote.NotFound, "مظنه یافت نشد."));

            if (quote.UserId != command.UserId)
                return Result<ConfirmQuoteResponse>.Failure(
                    Error.Failure(ErrorCodes.Auth.Unauthorized, "دسترسی غیرمجاز"));

            if (quote.Status != QuoteStatus.Active)
                return Result<ConfirmQuoteResponse>.Failure(
                    Error.Failure(ErrorCodes.Quote.IsNotActive, "مظنه فعال نیست."));

            if (now > quote.ExpiredAtUtc)
            {
                quote.Expire(now);
                await _unitOfWork.SaveChangeAsync(cancellationToken);

                return Result<ConfirmQuoteResponse>.Failure(
                    Error.Failure(ErrorCodes.Quote.Expired, "مظنه منقضی شده."));
            }

            var wallet = await _walletRepository.GetByUserIdAsync(
                command.UserId,
                cancellationToken);

            if (wallet is null)
                return Result<ConfirmQuoteResponse>.Failure(
                    Error.Failure(ErrorCodes.Wallet.NotFound, "کیف پول یافت نشد."));

            var deficit = quote.TotalPrice - wallet.Balance;

            if (deficit <= 0)
            {
                wallet.Debit(
                    quote.TotalPrice,
                    $"Payment for quote {quote.Id}");

                quote.Confirm(now);

                var order = Order.Create(
                    quote.UserId,
                    quote.ProductId,
                    quote.ProductType,
                    quote.Side,
                    quote.RequestAmount,
                    quote.UnitPrice,
                    quote.TotalPrice,
                    quote.Id,
                    command.IdempotencyKey
                );

                await _orderRepository.AddAsync(order, cancellationToken);

                await _unitOfWork.SaveChangeAsync(cancellationToken);

                return new ConfirmQuoteResponse
                {
                    OrderId = order.Id,
                    UnitPrice = quote.UnitPrice,
                    TotalPrice = quote.TotalPrice,
                };
            }

            var payment = Payment.Create(
                command.UserId,
                deficit,
                quote.Id,
                "Zarinpal");

            await _paymentRepository.AddAsync(payment, cancellationToken);

            var gatewayResult = await _paymentGateway.CreatePayment(
                payment.Id,
                payment.Amount,
                cancellationToken);

            payment.SetAuthority(gatewayResult.Authority);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return new ConfirmQuoteResponse
            {
                RequiresPayment = true,
                PaymentUrl = gatewayResult.PaymentUrl,
                UnitPrice = quote.UnitPrice,
                TotalPrice = quote.TotalPrice,
            };
        }
    }
}
