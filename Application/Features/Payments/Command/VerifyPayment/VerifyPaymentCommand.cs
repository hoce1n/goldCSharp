using Application.Abstractions.Messaging;
using Application.Common.Result;

namespace Application.Features.Payments.Command.VerifyPayment
{
    public sealed record VerifyPaymentCommand(
        string Authority,
        string PaymentId,
        string? Status
    ) : ICommand<Result<VerifyPaymentResponse>>;
}
