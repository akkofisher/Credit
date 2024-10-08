using Credit.Application.Models.DtoModels;
using FluentValidation;

namespace Credit.Application.Models.Validators
{
    public class PersonValidator : AbstractValidator<PersonDtoModel>
    {
        public PersonValidator()
        {
            //first name and last name min 2 max 50 required, only characters , Georgian  letters or  English Letters, not a mix of both
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50);

            //personal number required,  exactly 11, only numbers
            RuleFor(x => x.PersonalNumber).NotEmpty()
                .Matches(@"^[0-9]*$").WithMessage("Only numbers")
                .Length(11);


            RuleFor(x => x.DateOfBirth).NotEmpty()
                .LessThan(x => DateOnly.FromDateTime(DateTime.Now.AddYears(-18)));

            RuleFor(x => x.Email)
                 .NotEmpty().EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(30);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .Equal(x => x.Password);
        }
    }
}
