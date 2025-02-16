using WebApp.Core.Models;

namespace WebApp.Core.Repositories;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Product { get; }
    IRepository<Category, int> Category { get; }
    IRepository<Supplier, int> Supplier { get; }
    Task<int> SaveAsync();

    // Transactions
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}