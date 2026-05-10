using FluentValidation;

namespace Application.Features.Catalog.Coins.Commands.DeactivateCoin
{
    public class DeactivateCoinCommandValidator : AbstractValidator<DeactivateCoinCommand>
    {
        public DeactivateCoinCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
