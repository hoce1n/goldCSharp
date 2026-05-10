using FluentValidation;

namespace Application.Features.Catalog.Coins.Commands.UpdateStock
{
    public class UpdateCoinStockCommandValidator : AbstractValidator<UpdateCoinStockCommand>
    {
        public UpdateCoinStockCommandValidator() 
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("موجودی نمیتواند منفی باشد.");
        }
    }
}
