using Domain.Enums.Karat;
using FluentValidation;

namespace Application.Features.Catalog.Coins.Commands.CreateCoin
{
    public sealed class CreateCoinCommandValidator
        : AbstractValidator<CreateCoinCommand>
    {
        public CreateCoinCommandValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("نام سکه الزامی است")
            .MaximumLength(100).WithMessage("نام نمی‌تواند بیشتر از 100 کاراکتر باشد.");


            RuleFor(x => x.WeightInSoot)
                .GreaterThan(0).WithMessage("وزن سکه باید بیشتر از صفر باشد.");

            RuleFor(x => x.Karat)
                .IsInEnum().WithMessage("عیار وارد شده معتبر نیست.")
                .Must(karat => karat is KaratType.K18 or KaratType.K21 or KaratType.K22 or KaratType.K24)
                .WithMessage("عیار فقط می‌تواند 18، 21، 22 یا 24 باشد.");

            RuleFor(x => x.MintingFee)
                .GreaterThanOrEqualTo(0)
                .WithMessage("حق ضرب نمی‌تواند منفی باشد.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("موجودی اولیه نمی‌تواند منفی باشد.");
        }
    }
}
