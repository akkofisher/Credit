using Credit.Domain.Entities;
using Credit.Domain.Enums;

namespace Credit.Application.Models.ViewModels
{
    public class CreditViewModel
    {
        public int Id { get; set; }
        public decimal RequestAmount { get; set; }
        public CurrencyEnum Currency { get; set; }
        public DateOnly PeriodStart { get; set; }
        public DateOnly PeriodEnd { get; set; }

        public CreditStatusEnum Status { get; set; }

        public int PersonId { get; set; }
        public CreditPersonViewModel Person { get; set; }
    }
}
