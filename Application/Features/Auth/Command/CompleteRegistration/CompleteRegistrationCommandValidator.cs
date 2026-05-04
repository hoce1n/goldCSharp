using FluentValidation;

namespace Application.Features.Auth.Command.CompleteRegistration
{
    public sealed class CompleteRegistrationCommandValidator
        : AbstractValidator<CompleteRegistrationCommand>
    {
        public CompleteRegistrationCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.NationalCode)
                .NotEmpty()
                .Length(10)
                .Matches(@"^\d{10}$")
                .WithMessage("National code must be 10 digits.");

            RuleFor(x => x.Birthdate)
                .NotEmpty()
                .LessThan(DateTime.UtcNow)
                .WithMessage("Birthdate must be in the past.");

            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email));
        }

    }
}
