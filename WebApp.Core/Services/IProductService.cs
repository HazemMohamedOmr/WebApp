using WebApp.Core.DTOs;
using WebApp.Core.Models;

namespace WebApp.Core.Services;

internal interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(int id);
    Task<Product> AddAsync(ProductDTO productDto);
    Task<Product> UpdateAsync(int id, ProductDTO productDto);
    Task<bool> DeleteAsync(int id);
    
}