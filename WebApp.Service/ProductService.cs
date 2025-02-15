using Microsoft.AspNetCore.Http;
using WebApp.Core.DTOs;
using WebApp.Core.Interfaces;
using WebApp.Core.Models;
using WebApp.Core.Repositories;
using WebApp.Core.Services;
using WebApp.Infrastructure.Exceptions;

namespace WebApp.Service;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IServiceResponse<IEnumerable<Product>>> GetAllAsync()
    {
        var products = await _unitOfWork.Product.GetAllAsync();
        return ServiceResponse<IEnumerable<Product>>.Success(products);
    }

    public async Task<IServiceResponse<Product>> GetByIdAsync(int id)
    {
        var product = await _unitOfWork.Product.GetByIdAsync(id);
        if (product == null)
            return ServiceResponse<Product>.Fail("Product not Found", StatusCodes.Status404NotFound);
        return ServiceResponse<Product>.Success(product);
    }

    public async Task<IServiceResponse<Product>> AddAsync(ProductDTO productDTO)
    {
        var anyCategoryTask = await _unitOfWork.Category.AnyAsync(u => u.Id == productDTO.CategoryId);
        if (anyCategoryTask is false)
            return ServiceResponse<Product>.Fail("Category doesn't exist");

        var anySupplierTask = await _unitOfWork.Supplier.AnyAsync(s => s.Id == productDTO.SupplierId);
        if (anySupplierTask is false)
            return ServiceResponse<Product>.Fail("Supplier doesn't exist"); ;

        //var anyCategoryTask = _unitOfWork.Category.AnyAsync(u => u.Id == productDTO.CategoryId);
        //var anySupplierTask = _unitOfWork.Supplier.AnyAsync(s => s.Id == productDTO.SupplierId);

        //var tasks = await Task.WhenAll(anyCategoryTask, anySupplierTask);
        //if (tasks.Any(u => u == false))
        //    return null;

        var product = new Product()
        {
            Name = productDTO.Name.Trim(),
            Description = productDTO.Description?.Trim(),
            Price = productDTO.Price,
            CategoryId = productDTO.CategoryId,
            SupplierId = productDTO.SupplierId,
        };
        var createdProduct = await _unitOfWork.Product.AddAsync(product);
        await _unitOfWork.SaveAsync();
        return ServiceResponse<Product>.Success(createdProduct.Entity);
    }

    public async Task<IServiceResponse<Product>> UpdateAsync(int id, ProductDTO productDTO)
    {
        var product = await _unitOfWork.Product.GetByIdAsync(id);
        if (product is null)
            return ServiceResponse<Product>.Fail("Product not Found", StatusCodes.Status404NotFound);

        product.Name = productDTO.Name.Trim();
        product.Description = productDTO.Description?.Trim();
        product.Price = productDTO.Price;

        await _unitOfWork.SaveAsync();
        return ServiceResponse<Product>.Success(product);
    }

    public async Task<IServiceResponse<bool>> DeleteAsync(int id)
    {
        var product = await _unitOfWork.Product.GetByIdAsync(id).ConfigureAwait(false);
        if (product is null)
            return ServiceResponse<bool>.Fail("Product not Found", StatusCodes.Status404NotFound);
        _unitOfWork.Product.Delete(product);
        await _unitOfWork.SaveAsync();
        return ServiceResponse<bool>.Success(true);
    }
}