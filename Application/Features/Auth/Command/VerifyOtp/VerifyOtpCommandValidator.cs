using FluentValidation;

namespace Application.Features.Auth.Command.VerifyOtp
{
    public class VerifyOtpCommandValidator : AbstractValidator<VerifyOtpCommand>
    {
        public VerifyOtpCommandValidator() 
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{10,15}$").WithMessage("Invalid phone number format.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("OTP code is required.")
                .Length(6).WithMessage("OTP must be between 4 and 6 digits.")
                .Matches(@"^\d+$").WithMessage("OTP must contain only digits.");
        }
    }
}
