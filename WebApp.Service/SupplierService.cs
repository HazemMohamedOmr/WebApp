using Microsoft.AspNetCore.Http;
using WebApp.Core.DTOs;
using WebApp.Core.Interfaces;
using WebApp.Core.Models;
using WebApp.Core.Repositories;
using WebApp.Core.Services;
using WebApp.Infrastructure.Exceptions;

namespace WebApp.Service;

public class SupplierService : ISupplierService
{
    private readonly IUnitOfWork _unitOfWork;

    public SupplierService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IServiceResponse<IEnumerable<Supplier>>> GetAllAsync()
    {
        var suppliers = await _unitOfWork.Supplier.GetAllAsync();
        return ServiceResponse<IEnumerable<Supplier>>.Success(suppliers);
    }

    public async Task<IServiceResponse<Supplier>> GetByIdAsync(int id)
    {
        var supplier = await _unitOfWork.Supplier.GetByIdAsync(id);
        if (supplier is null)
            return ServiceResponse<Supplier>.Fail("Supplier not Found", StatusCodes.Status404NotFound);
        return ServiceResponse<Supplier>.Success(supplier);
    }

    public async Task<IServiceResponse<Supplier>> AddAsync(SupplierDTO supplierDto)
    {
        var supplier = new Supplier()
        {
            CompanyName = supplierDto.CompanyName.Trim(),
            ContactInfo = supplierDto.ContactInfo?.Trim(),
        };
        var createdSupplier = await _unitOfWork.Supplier.AddAsync(supplier);
        await _unitOfWork.SaveAsync();
        return ServiceResponse<Supplier>.Success(createdSupplier.Entity);
    }

    public async Task<IServiceResponse<Supplier>> UpdateAsync(int id, SupplierDTO supplierDto)
    {
        var supplier = await _unitOfWork.Supplier.GetByIdAsync(id);
        if (supplier is null)
            return ServiceResponse<Supplier>.Fail("Supplier not Found", StatusCodes.Status404NotFound);

        supplier.CompanyName = supplierDto.CompanyName.Trim();
        supplier.ContactInfo = supplierDto.ContactInfo?.Trim();

        await _unitOfWork.SaveAsync();
        return ServiceResponse<Supplier>.Success(supplier);
    }

    public async Task<IServiceResponse<bool>> DeleteAsync(int id)
    {
        var supplier = await _unitOfWork.Supplier.GetByIdAsync(id).ConfigureAwait(false);
        if (supplier is null)
            return ServiceResponse<bool>.Fail("Supplier not Found", StatusCodes.Status404NotFound); ;

        _unitOfWork.Supplier.Delete(supplier);
        await _unitOfWork.SaveAsync();

        return ServiceResponse<bool>.Success(true);
    }
}