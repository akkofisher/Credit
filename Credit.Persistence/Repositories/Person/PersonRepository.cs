using Credit.Infrastructure.Persistence;
using Credit.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Credit.Infrastructure.Repositories.Person
{
    public sealed class PersonRepository : GenericRepository<PersonEntity>, IPersonRepository
    {
        public PersonRepository(MainDbContext mainDbContext) : base(mainDbContext)
        {
        }

        public async Task<PersonEntity> GetByEmail(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Email == email);
        }

        public async Task<PersonEntity> GetByPersonalNumber(string personalNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.IsDeleted == false && x.PersonalNumber == personalNumber);
        }

        public async Task<IEnumerable<PersonEntity>> GetPersonNamesByIds(List<int> personIds)
        {
            return await _dbSet.AsNoTracking()
                .Where(x =>
                    x.IsDeleted == false &&
                    personIds.Contains(x.Id))
                .Select(x => new PersonEntity
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName
                }).ToListAsync();
        }
    }

}
