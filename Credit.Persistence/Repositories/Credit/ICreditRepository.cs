using Credit.Domain.Entities;

namespace Credit.Infrastructure.Repositories.Credit
{
    public interface ICreditRepository : IGenericRepository<CreditEntity>
    {
        Task<CreditEntity?> GetCreditById(int id);
        Task<CreditEntity?> GetCreditByIdAndPersonId(int id, int personId);
        Task<IEnumerable<CreditEntity>> GetCreditsByPersonId(int personId);
        Task<IEnumerable<CreditEntity>> GetCreditsByStatus(CreditStatusEnum status);
    }
}
