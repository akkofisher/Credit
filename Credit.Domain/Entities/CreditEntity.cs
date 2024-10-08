using System.ComponentModel.DataAnnotations.Schema;
using Credit.Domain.Entities.Common;
using Credit.Domain.Enums;

namespace Credit.Domain.Entities
{
    public class CreditEntity : CommonEntity
    {
        public decimal RequestAmount { get; set; }
        public CurrencyEnum Currency { get; set; }
        public DateOnly PeriodStart { get; set; }
        public DateOnly PeriodEnd { get; set; }

        //admin only - default status is Sended
        public CreditStatusEnum Status { get; set; }

        public int PersonId { get; set; }
        [ForeignKey("PersonId")]
        public PersonEntity Person { get; set; }
    }
}
