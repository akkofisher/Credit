namespace Credit.Infrastructure.Repositories;

public interface IUnitOfWork : IDisposable
{
    public Task<bool> CommitAsync();
}