using Credit.Domain.Entities;
using Credit.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Credit.Infrastructure.Repositories.Credit
{
    public sealed class CreditRepository : GenericRepository<CreditEntity>, ICreditRepository
    {
        public CreditRepository(MainDbContext mainDbContext) : base(mainDbContext)
        {
        }

        //get credit by id
        public async Task<CreditEntity?> GetCreditById(int id)
        {
            return await _dbSet.Where(x => x.IsDeleted == false).FirstOrDefaultAsync(x => x.Id == id);
        }

        //get credit by id and person id
        public async Task<CreditEntity?> GetCreditByIdAndPersonId(int id, int personId)
        {
            return await _dbSet.Where(x => x.IsDeleted == false && x.PersonId == personId)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        //get all credits by person id
        public async Task<IEnumerable<CreditEntity>> GetCreditsByPersonId(int personId)
        {
            return await _dbSet.Where(x => x.IsDeleted == false && x.PersonId == personId)
                .Include(x => x.Person)
                .Select(x => new CreditEntity
                {
                    Id = x.Id,
                    PersonId = x.PersonId,
                    Status = x.Status,
                    PeriodStart = x.PeriodStart,
                    PeriodEnd = x.PeriodEnd,
                    Currency = x.Currency,
                    RequestAmount = x.RequestAmount,
                    Person = new PersonEntity
                    {
                        FirstName = x.Person.FirstName,
                        LastName = x.Person.LastName
                    }
                })
                .ToListAsync();
        }

        //get all credits by status
        public async Task<IEnumerable<CreditEntity>> GetCreditsByStatus(CreditStatusEnum status)
        {
            return await _dbSet.Where(x => x.IsDeleted == false && x.Status == status).ToListAsync();
        }
    }

}
