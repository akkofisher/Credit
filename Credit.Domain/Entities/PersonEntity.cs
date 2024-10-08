using Credit.Domain.Entities.Common;
using Credit.Domain.Enums;

namespace Credit.Domain.Entities
{
    public class PersonEntity : CommonEntity
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PasswordHash { get; set; }

        public RoleEnum Role { get; set; }

        public ICollection<CreditEntity> Credits { get; set; }
    }
}
