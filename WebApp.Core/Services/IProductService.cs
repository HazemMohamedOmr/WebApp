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
    Task<IServiceResponse<Product>> GetProductData(int id);
    Task<IServiceResponse<EntityPaged<Product>>> GetProductsWithPages(int pageNumber, int pageSize);
    Task<IServiceResponse<IEnumerable<Product>>> GetProductsFilterd();
    Task<IServiceResponse<EntityPaged<Product>>> GetProductsFilterdWithPages(int pageNumber, int pageSize);
    Task<IServiceResponse<IEnumerable<Product>>> GetAllProductsSorted();
    Task<IServiceResponse<IEnumerable<Product>>> GetProductsFilterdAndSorted();
    Task<IServiceResponse<int>> GetProductsCounts();
    Task<IServiceResponse<decimal>> GetProductsMax();
    Task<IServiceResponse<decimal>> GetProductsMin();
    Task<IServiceResponse<decimal>> GetProductsSum();
    Task<IServiceResponse<decimal>> GetProductsAverage();
}