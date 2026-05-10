using Application.Abstractions.Messaging;
using Application.Common.Result;

namespace Application.Features.Auth.Command.VerifyOtp
{
    public record VerifyOtpCommand(string PhoneNumber, string Code) 
        : ICommand<Result<VerifyOtpResponse>>;
}
