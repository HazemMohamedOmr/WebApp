using WebApp.Core.DTOs;
using WebApp.Core.Interfaces;
using WebApp.Core.Models;

namespace WebApp.Core.Services;

public interface ISupplierService
{
    Task<IServiceResponse<IEnumerable<Supplier>>> GetAllAsync();
    Task<IServiceResponse<Supplier>> GetByIdAsync(int id);
    Task<IServiceResponse<Supplier>> AddAsync(SupplierDTO supplierDto);
    Task<IServiceResponse<Supplier>> UpdateAsync(int id, SupplierDTO supplierDto);
    Task<IServiceResponse<bool>> DeleteAsync(int id);
}