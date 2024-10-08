using MediatR;

namespace Credit.Application.Models.DtoModels
{
    public class PersonDtoModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
