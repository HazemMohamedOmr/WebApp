using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApp.Core.Constants;

namespace WebApp.Core.Repositories;

public interface IRepository<T> where T : class
{
    // Query
    Task<T> GetByIdAsync<TId>(TId id);
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);

    Task<(IEnumerable<T> Items, int TotalCount)> GetAllPagedAsync(int pageNumber, int pageSize,
        params Expression<Func<T, object>>[] includes);

    Task<IEnumerable<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes);

    Task<(IEnumerable<T> Items, int TotalCount)> GetFilteredPagedAsync(Expression<Func<T, bool>> predicate,
        int pageNumber, int pageSize, params Expression<Func<T, object>>[] includes);

    // Aggregations
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    Task<decimal> SumAsync(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>>? predicate = null);
    Task<decimal> MaxAsync(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>>? predicate = null);
    Task<decimal> MinAsync(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>>? predicate = null);
    Task<decimal> AverageAsync(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>>? predicate = null);

    // Logical Operations
    Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null);
    Task<bool> AllAsync(Expression<Func<T, bool>> predicate);

    // Create / Update / Delete
    Task<EntityEntry<T>> AddAsync(T entity);
    IEnumerable<T> AddRange(IEnumerable<T> entities);
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
    T Update(T entity);
    IEnumerable<T> UpdateRange(IEnumerable<T> entities);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);

    // Single Record
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

    // Sorting
    Task<IEnumerable<T>> GetAllSortedAsync<TKey>(Expression<Func<T, TKey>> orderBy,
        string orderByDirection = GeneralConstants.OrderBy.Ascending);

    Task<IEnumerable<T>> GetFilteredSortedAsync<TKey>(Expression<Func<T, bool>> predicate,
        Expression<Func<T, TKey>> orderBy, string orderByDirection = GeneralConstants.OrderBy.Ascending,
        params Expression<Func<T, object>>[] includes);

    // Projection
    Task<IEnumerable<TResult>> ProjectAsync<TResult>(Expression<Func<T, TResult>> selector,
        params Expression<Func<T, object>>[] includes);

    // Bulk Operations
    Task AddBulkAsync(IEnumerable<T> entities);
    Task UpdateBulkAsync(IEnumerable<T> entities);
    Task DeleteBulkAsync(IEnumerable<T> entities);
}