using Credit.Domain.Entities;

namespace Credit.Application.Models.DtoModels
{
    public class ChangeOrderCreditStatusDtoModel
    {
        public int Id { get; set; }

        public CreditStatusEnum Status { get; set; }
    }
}
