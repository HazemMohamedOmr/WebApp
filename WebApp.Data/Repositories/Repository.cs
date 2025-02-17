using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApp.Core.Constants;
using WebApp.Core.Interfaces;
using WebApp.Core.Models;
using WebApp.Core.Repositories;
using WebApp.Data.Data;

namespace WebApp.Data.Repositories;

public class Repository<T, TId> : IRepository<T, TId> where T : BaseEntity<TId>
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    // ---------------------- READ OPERATIONS (NO TRACKING) ----------------------

    public async Task<T> GetByIdAsync(TId id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
    {
        return await ApplyIncludes(_dbSet.AsNoTracking(), includes).ToListAsync();
    }

    public async Task<(IEnumerable<T> Items, int TotalCount)> GetAllPagedAsync(int pageNumber, int pageSize,
        params Expression<Func<T, object>>[] includes)
    {
        var query = ApplyIncludes(_dbSet.AsNoTracking(), includes);
        var totalCount = await query.CountAsync();
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<IEnumerable<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes)
    {
        return await ApplyIncludes(_dbSet.AsNoTracking().Where(predicate), includes).ToListAsync();
    }

    public async Task<(IEnumerable<T> Items, int TotalCount)> GetFilteredPagedAsync(Expression<Func<T, bool>> predicate,
        int pageNumber, int pageSize, params Expression<Func<T, object>>[] includes)
    {
        var query = ApplyIncludes(_dbSet.AsNoTracking().Where(predicate), includes);
        var totalCount = await query.CountAsync();
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes)
    {
        return await ApplyIncludes(_dbSet.AsNoTracking(), includes).FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<T>> GetAllSortedAsync<TKey>(Expression<Func<T, TKey>> orderBy,
        string orderByDirection = GeneralConstants.OrderBy.Ascending, params Expression<Func<T, object>>[] includes)
    {
        var query = ApplyIncludes(_dbSet.AsNoTracking(), includes);
        return orderByDirection == GeneralConstants.OrderBy.Ascending
            ? await query.OrderBy(orderBy).ToListAsync()
            : await query.OrderByDescending(orderBy).ToListAsync();
    }

    public async Task<IEnumerable<T>> GetFilteredSortedAsync<TKey>(Expression<Func<T, bool>> predicate,
        Expression<Func<T, TKey>> orderBy, string orderByDirection = GeneralConstants.OrderBy.Ascending,
        params Expression<Func<T, object>>[] includes)
    {
        var query = ApplyIncludes(_dbSet.AsNoTracking().Where(predicate), includes);
        return orderByDirection == GeneralConstants.OrderBy.Ascending
            ? await query.OrderBy(orderBy).ToListAsync()
            : await query.OrderByDescending(orderBy).ToListAsync();
    }

    // ---------------------- AGGREGATE FUNCTIONS ----------------------

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        return await (predicate == null ? _dbSet.CountAsync() : _dbSet.CountAsync(predicate));
    }

    public async Task<decimal> SumAsync(Expression<Func<T, decimal>> selector,
        Expression<Func<T, bool>>? predicate = null)
    {
        return await (predicate == null ? _dbSet.SumAsync(selector) : _dbSet.Where(predicate).SumAsync(selector));
    }

    public async Task<decimal> MaxAsync(Expression<Func<T, decimal>> selector,
        Expression<Func<T, bool>>? predicate = null)
    {
        return await (predicate == null ? _dbSet.MaxAsync(selector) : _dbSet.Where(predicate).MaxAsync(selector));
    }

    public async Task<decimal> MinAsync(Expression<Func<T, decimal>> selector,
        Expression<Func<T, bool>>? predicate = null)
    {
        return await (predicate == null ? _dbSet.MinAsync(selector) : _dbSet.Where(predicate).MinAsync(selector));
    }

    public async Task<decimal> AverageAsync(Expression<Func<T, decimal>> selector,
        Expression<Func<T, bool>>? predicate = null)
    {
        return await (predicate == null
            ? _dbSet.AverageAsync(selector)
            : _dbSet.Where(predicate).AverageAsync(selector));
    }

    // ---------------------- LOGICAL OPERATIONS ----------------------

    public async Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null)
    {
        return await (predicate == null ? _dbSet.AnyAsync() : _dbSet.AnyAsync(predicate));
    }

    public async Task<bool> AllAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AllAsync(predicate);
    }

    // ---------------------- CUD OPERATIONS ----------------------

    public async Task<EntityEntry<T>> AddAsync(T entity)
    {
        return await _dbSet.AddAsync(entity);
    }

    public IEnumerable<T> AddRange(IEnumerable<T> entities)
    {
        _dbSet.AddRange(entities);
        return entities;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        return entities;
    }

    public T Update(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
        return entity;
    }

    public IEnumerable<T> UpdateRange(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _dbSet.UpdateRange(entities);
        return entities;
    }

    public void Delete(T entity)
    {
        if (entity is ISoftDeletable softDeletableEntity)
        {
            softDeletableEntity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            _dbSet.Remove(entity); // Hard delete if not soft-deletable
        }
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            if (entity is ISoftDeletable softDeletableEntity)
            {
                softDeletableEntity.IsDeleted = true;
                entity.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                _dbSet.Remove(entity); // Hard delete if not soft-deletable
            }
        }
    }

    // ---------------------- PROJECTION ----------------------

    public async Task<IEnumerable<TResult>> ProjectAsync<TResult>(Expression<Func<T, TResult>> selector,
        params Expression<Func<T, object>>[] includes)
    {
        return await ApplyIncludes(_dbSet.AsNoTracking(), includes).Select(selector).ToListAsync();
    }

    // ---------------------- BULK OPERATIONS ----------------------

    public async Task AddBulkAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities); // TODO Adjust Bulking Operation - Add
    }

    public async Task UpdateBulkAsync(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities); // TODO Adjust Bulking Operation - Update
    }

    public async Task DeleteBulkAsync(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities); // TODO Adjust Bulking Operation - Delete
    }

    // ---------------------- PRIVATE HELPER METHOD ----------------------

    private IQueryable<T> ApplyIncludes(IQueryable<T> query, params Expression<Func<T, object>>[] includes)
    {
        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);

        return query;
    }
}