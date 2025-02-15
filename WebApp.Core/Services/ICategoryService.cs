using WebApp.Core.DTOs;
using WebApp.Core.Interfaces;
using WebApp.Core.Models;

namespace WebApp.Core.Services;

public interface ICategoryService
{
    Task<IServiceResponse<IEnumerable<Category>>> GetAllAsync();
    Task<IServiceResponse<Category>> GetByIdAsync(int id);
    Task<IServiceResponse<Category>> AddAsync(CategoryDTO categoryDto);
    Task<IServiceResponse<Category>> UpdateAsync(int id, CategoryDTO categoryDto);
    Task<IServiceResponse<bool>> DeleteAsync(int id);
}