using Domain.Enums;
using FluentValidation;

namespace Application.Features.Catalog.Coins.Commands.UpdateCoin
{
    public class UpdateCoinCommandValidator : AbstractValidator<UpdateCoinCommand>
    {
        public UpdateCoinCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.WeightInSoot)
                .GreaterThan(0);

            RuleFor(x => x.Karat)
                .IsInEnum();

            RuleFor(x => x.ImageUrl)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.ImageUrl));

            RuleFor(x => x.Description)
                .MaximumLength(2000)
                .When(x => !string.IsNullOrWhiteSpace(x.Description));
        }
    }
}
