using FluentValidation;

namespace Application.Features.Auth.Command.SendOtp
{
    public sealed class SendOtpCommandValidator 
        : AbstractValidator<SendOtpCommand>
    {
        public SendOtpCommandValidator() 
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches("^09[0-9]{9}$").WithMessage("Phone number is not valid.");
        }
    }
}
