namespace Application.Features.Payments.Command.VerifyPayment
{
    public sealed record VerifyPaymentResponse(
        bool IsSuccess,
        string Message,
        Guid? OrderId = null,
        decimal? PaidAmount = null
    );
}
