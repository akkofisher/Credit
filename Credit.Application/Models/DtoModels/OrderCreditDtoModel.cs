using Credit.Domain.Enums;

namespace Credit.Application.Models.DtoModels
{
    public class OrderCreditDtoModel
    {
        public int? Id { get; set; }
        public decimal RequestAmount { get; set; }
        public CurrencyEnum Currency { get; set; }
        public DateOnly PeriodStart { get; set; }
        public DateOnly PeriodEnd { get; set; }
    }
}
