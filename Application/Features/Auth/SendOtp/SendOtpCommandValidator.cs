using FluentValidation;

namespace Application.Features.Auth.SendOtp
{
    public sealed class SendOtpCommandValidator 
        : AbstractValidator<SendOtpCommand>
    {
        public SendOtpCommandValidator() 
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches("^09[0-9]{9}$")
                .WithMessage("Phone number is not valid.");
        }
    }
}
