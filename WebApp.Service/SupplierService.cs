using WebApp.Core.DTOs;
using WebApp.Core.Models;
using WebApp.Core.Repositories;
using WebApp.Core.Services;

namespace WebApp.Service;

public class SupplierService : ISupplierService
{
    private readonly IUnitOfWork _unitOfWork;

    public SupplierService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        return await _unitOfWork.Supplier.GetAllAsync();
    }

    public async Task<Supplier> GetByIdAsync(int id)
    {
        return await _unitOfWork.Supplier.GetByIdAsync(id);
    }

    public async Task<Supplier> AddAsync(SupplierDTO supplierDto)
    {
        var supplier = new Supplier()
        {
            CompanyName = supplierDto.CompanyName.Trim(),
            ContactInfo = supplierDto.ContactInfo?.Trim(),
        };
        var createdSupplier = await _unitOfWork.Supplier.AddAsync(supplier);
        await _unitOfWork.SaveAsync();
        return createdSupplier.Entity;
    }

    public async Task<Supplier> UpdateAsync(int id, SupplierDTO supplierDto)
    {
        var supplier = await _unitOfWork.Supplier.GetByIdAsync(id);
        if (supplier is null)
            return supplier;

        supplier.CompanyName = supplierDto.CompanyName.Trim();
        supplier.ContactInfo = supplierDto.ContactInfo?.Trim();

        await _unitOfWork.SaveAsync();
        return supplier;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var supplier = await _unitOfWork.Supplier.GetByIdAsync(id).ConfigureAwait(false);
        if (supplier is null)
            return false;

        _unitOfWork.Supplier.Delete(supplier);
        await _unitOfWork.SaveAsync();

        return true;
    }
}