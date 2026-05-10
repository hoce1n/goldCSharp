using FluentValidation;

namespace Application.Features.Catalog.Coins.Commands.ActivateCoin
{
    public class ActivateCoinCommandValidator : AbstractValidator<ActivateCoinCommand>
    {
        public ActivateCoinCommandValidator() {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
