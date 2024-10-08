using Credit.Application.Models.DtoModels;
using FluentValidation;

namespace Credit.Application.Models.Validators
{
    public class ChangeOrderCreditStatusValidator : AbstractValidator<ChangeOrderCreditStatusDtoModel>
    {
        public ChangeOrderCreditStatusValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
            RuleFor(x => x.Status).NotEmpty().IsInEnum().WithMessage("Status is required");
        }
    }
}
