using WebApp.Core.Models;

namespace WebApp.Core.Repositories;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Product { get; }
    IRepository<Category> Category { get; }
    IRepository<Supplier> Supplier { get; }
    Task<int> SaveAsync();

    // Transactions
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}