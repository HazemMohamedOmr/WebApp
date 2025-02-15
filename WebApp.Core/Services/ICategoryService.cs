using WebApp.Core.DTOs;
using WebApp.Core.Models;

namespace WebApp.Core.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category> GetByIdAsync(int id);
    Task<Category> AddAsync(CategoryDTO categoryDto);
    Task<Category> UpdateAsync(int id, CategoryDTO categoryDto);
    Task<bool> DeleteAsync(int id);
}