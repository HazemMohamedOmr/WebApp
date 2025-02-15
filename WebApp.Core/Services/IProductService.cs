using WebApp.Core.DTOs;
using WebApp.Core.Interfaces;
using WebApp.Core.Models;

namespace WebApp.Core.Services;

public interface IProductService
{
    Task<IServiceResponse<IEnumerable<Product>>> GetAllAsync();
    Task<IServiceResponse<Product>> GetByIdAsync(int id);
    Task<IServiceResponse<Product>> AddAsync(ProductDTO productDTO);
    Task<IServiceResponse<Product>> UpdateAsync(int id, ProductDTO productDTO);
    Task<IServiceResponse<bool>> DeleteAsync(int id);
}