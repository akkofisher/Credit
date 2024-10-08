namespace Credit.Application.Models.ViewModels
{
    public class PersonViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
