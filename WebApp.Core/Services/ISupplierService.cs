using WebApp.Core.DTOs;
using WebApp.Core.Models;

namespace WebApp.Core.Services;

public interface ISupplierService
{
    Task<IEnumerable<Supplier>> GetAllAsync();
    Task<Supplier> GetByIdAsync(int id);
    Task<Supplier> AddAsync(SupplierDTO supplierDto);
    Task<Supplier> UpdateAsync(int id, SupplierDTO supplierDto);
    Task<bool> DeleteAsync(int id);
}