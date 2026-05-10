using FluentValidation;

namespace Application.Features.Catalog.Coins.Commands.DeleteCoin
{
    public class DeleteCoinCommandValidator : AbstractValidator<DeleteCoinCommand>
    {
        public DeleteCoinCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

}
