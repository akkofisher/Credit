using Credit.Application.Models.DtoModels;
using FluentValidation;

namespace Credit.Application.Models.Validators
{
    public class OrderCreditValidator : AbstractValidator<OrderCreditDtoModel>
    {
        public OrderCreditValidator()
        {
            RuleFor(x => x.RequestAmount)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.Currency)
                .NotEmpty()
                .IsInEnum();

            RuleFor(x => x.PeriodStart)
                .NotEmpty()
                .LessThan(x => x.PeriodEnd);

            RuleFor(x => x.PeriodEnd)
                .NotEmpty()
                .GreaterThan(x => x.PeriodStart);
        }
    }
}
