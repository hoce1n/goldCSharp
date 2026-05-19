using Application.Abstractions.Messaging;
using Application.Common.Result;

namespace Application.Features.Payments.Command.CreatePayment
{
    public record CreatePaymentCommand(
        Guid UserId,
        decimal Amount
    ) : ICommand<Result<CreatePaymentResponse>>;
}
