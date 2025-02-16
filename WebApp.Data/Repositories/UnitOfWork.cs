using Microsoft.EntityFrameworkCore.Storage;
using WebApp.Core.Models;
using WebApp.Core.Repositories;
using WebApp.Data.Data;

namespace WebApp.Data.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _context;
    private IRepository<Category, int> _category;
    private IRepository<Supplier, int> _supplier;

    private IProductRepository _product;
    private IDbContextTransaction _transaction;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IProductRepository Product => _product ??= new ProductRepository(_context);
    public IRepository<Category, int> Category => _category ??= new Repository<Category, int>(_context);
    public IRepository<Supplier, int> Supplier => _supplier ??= new Repository<Supplier, int>(_context);

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    // ---------------------- TRANSACTIONS ----------------------

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.SaveChangesAsync();
        await _transaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}