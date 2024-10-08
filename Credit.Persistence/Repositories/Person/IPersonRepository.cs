using Credit.Domain.Entities;

namespace Credit.Infrastructure.Repositories.Person
{
    public interface IPersonRepository : IGenericRepository<PersonEntity>
    {
        Task<PersonEntity> GetByEmail(string email);
        Task<PersonEntity> GetByPersonalNumber(string personalNumber);
        Task<IEnumerable<PersonEntity>> GetPersonNamesByIds(List<int> personIds);
    }
}
