using Credit.Application.Models.DtoModels;
using FluentValidation;

namespace Credit.Application.Models.Validators
{

    public class LoginPersonValidator : AbstractValidator<LoginPersonDtoModel>
    {
        public LoginPersonValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(30);
        }
    }
}
