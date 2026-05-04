using Application.Abstractions.Messaging;
using Application.Common.Result;

namespace Application.Features.Auth.Command.SendOtp
{
    public sealed record SendOtpCommand(
        string PhoneNumber
    ) : ICommand<Result<SendOtpResponse>>;
}
