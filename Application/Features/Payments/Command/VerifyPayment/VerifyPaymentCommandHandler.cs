using Application.Abstractions.Messaging;
using Application.Abstractions.Payments;
using Application.Abstractions.Repositories;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Common.Errors;
using Domain.Entities.Order;

namespace Application.Features.Payments.Command.VerifyPayment
{
    public sealed class VerifyPaymentCommandHandler
        : ICommandHandler<VerifyPaymentCommand, Result<VerifyPaymentResponse>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IQuoteRepository _quoteRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public VerifyPaymentCommandHandler(
            IPaymentRepository paymentRepository,
            IQuoteRepository quoteRepository,
            IWalletRepository walletRepository,
            IOrderRepository orderRepository,
            IPaymentGateway paymentGateway,
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider)
        {
            _paymentRepository = paymentRepository;
            _quoteRepository = quoteRepository;
            _walletRepository = walletRepository;
            _orderRepository = orderRepository;
            _paymentGateway = paymentGateway;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<VerifyPaymentResponse>> Handle(
            VerifyPaymentCommand command,
            CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;
            var paymentId = Guid.Parse(command.PaymentId);

            var payment = await _paymentRepository.GetByIdAsync(paymentId, cancellationToken);

            if (payment is null)
                return Result<VerifyPaymentResponse>.Failure(
                    Error.Failure(ErrorCodes.Payment.NotFound, "پرداخت یافت نشد."));

            if (payment.IsSucceeded())
                return Result<VerifyPaymentResponse>.Failure(
                    Error.Failure(ErrorCodes.Payment.AlreadyVerified, "این پرداخت قبلا تایید شده است."));

            var verifyResult = await _paymentGateway.VerifyPayment(
                payment.Authority!,
                payment.Amount,
                cancellationToken);

            if (!verifyResult.IsSuccessful)
            {
                payment.MarkFailed(now);
                await _unitOfWork.SaveChangeAsync();
                return Result<VerifyPaymentResponse>.Failure(
                    Error.Failure(ErrorCodes.Payment.VerificationFailed, verifyResult.Message));
            }

            payment.MarkSucceeded(verifyResult.RefId, now);

            var wallet = await _walletRepository.GetByUserIdAsync(payment.UserId, cancellationToken);
            if (wallet is null)
                return Result<VerifyPaymentResponse>.Failure(
                    Error.Failure(ErrorCodes.Wallet.NotFound, "کیف پول یافت نشد."));

            wallet.Deposit(payment.Amount, $"شارژ کیف پول - پرداخت {payment.Id}");

            var quote = await _quoteRepository.GetByIdAsync(payment.QuoteId, cancellationToken);
            if (quote is null || quote.Status != Domain.Enums.Quote.QuoteStatus.Active)
            {
                await _unitOfWork.SaveChangeAsync(cancellationToken);
                return new VerifyPaymentResponse(true, "کیف پول شارژ شد، اما مظنه منقضی شده است.");
            }

            var currentBalance = wallet.Balance;
            if (currentBalance >= quote.TotalPrice)
            {
                wallet.Debit(quote.TotalPrice, $"خرید - مظنه {quote.Id}");
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
                    command.PaymentId
                );

                await _orderRepository.AddAsync(order, cancellationToken);
                await _unitOfWork.SaveChangeAsync(cancellationToken);

                return new VerifyPaymentResponse(
                    true,
                    "پرداخت با موفقیت انجام شد و سفارش ثبت گردید.",
                    order.Id,
                    payment.Amount
                );
            }

            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return new VerifyPaymentResponse(
                true,
                $"کیف پول به میزان {payment.Amount:N0} تومان شارژ شد. برای تکمیل خرید، مجدداً اقدام کنید.",
                null,
                payment.Amount
            );
        }
    }
}
