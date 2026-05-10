using FluentValidation;

namespace Application.Features.Catalog.Coins.Commands.UpdateMintingFee
{
    public class UpdateCoinMintingFeeCommandValidator
        : AbstractValidator<UpdateCoinMintingFeeCommand>
    {
        public UpdateCoinMintingFeeCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.MintingFee)
                .GreaterThanOrEqualTo(0)
                .WithMessage("اجرت نمی‌تواند منفی باشد.");
        }
    }
}
