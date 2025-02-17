using Microsoft.AspNetCore.Http;
using WebApp.Core.Constants;
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
        //var product = await _unitOfWork.Product.FirstOrDefaultAsync(x => x.Id == id, m => m.Category, x => x.Supplier);
        var product = await _unitOfWork.Product.GetByIdAsync(id);
        if (product is null)
            return ServiceResponse<Product>.Fail("Product not Found", StatusCodes.Status404NotFound);
        return ServiceResponse<Product>.Success(product);
    }

    public async Task<IServiceResponse<Product>> AddAsync(ProductDTO productDTO)
    {
        var anyCategoryTask = await _unitOfWork.Category.AnyAsync(u => u.Id == productDTO.CategoryId);
        if (anyCategoryTask is false)
            return ServiceResponse<Product>.Fail("Category doesn't exist", StatusCodes.Status400BadRequest);

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

    public async Task<IServiceResponse<Product>> GetProductData(int id)
    {
        var product = await _unitOfWork.Product.FirstOrDefaultAsync(x => x.Id == id, m => m.Category, x => x.Supplier);
        if (product is null)
            return ServiceResponse<Product>.Fail("Product not Found", StatusCodes.Status404NotFound);
        return ServiceResponse<Product>.Success(product);
    }

    public async Task<IServiceResponse<EntityPaged<Product>>> GetProductsWithPages(int pageNumber, int pageSize)
    {
        if (pageSize < 1 || pageNumber < 1)
            return ServiceResponse<EntityPaged<Product>>.Fail("Page Size or Number is Invaild", StatusCodes.Status400BadRequest);

        var result = await _unitOfWork.Product.GetAllPagedAsync(pageNumber, pageSize);
        if (result.TotalCount == 0)
            return ServiceResponse<EntityPaged<Product>>.Fail("Out Of Scope", StatusCodes.Status400BadRequest);

        var ProductsPaged = new EntityPaged<Product>
        {
            TotalCount = result.TotalCount,
            Items = result.Items
        };
        return ServiceResponse<EntityPaged<Product>>.Success(ProductsPaged);
    }

    public async Task<IServiceResponse<IEnumerable<Product>>> GetProductsFilterd()
    {
        var products = await _unitOfWork.Product.GetFilteredAsync(m => m.Price >= 50);
        if (products.Any() is false)
            return ServiceResponse<IEnumerable<Product>>.Fail("Products not Found", StatusCodes.Status404NotFound);

        return ServiceResponse<IEnumerable<Product>>.Success(products);
    }

    public async Task<IServiceResponse<EntityPaged<Product>>> GetProductsFilterdWithPages(int pageNumber, int pageSize)
    {
        if (pageSize < 1 || pageNumber < 1)
            return ServiceResponse<EntityPaged<Product>>.Fail("Page Size or Number is Invaild", StatusCodes.Status400BadRequest);

        var result = await _unitOfWork.Product.GetFilteredPagedAsync(x => x.Price >= 50, pageNumber, pageSize);
        if (result.TotalCount == 0)
            return ServiceResponse<EntityPaged<Product>>.Fail("Out Of Scope", StatusCodes.Status400BadRequest);

        var ProductsPaged = new EntityPaged<Product>
        {
            TotalCount = result.TotalCount,
            Items = result.Items
        };
        return ServiceResponse<EntityPaged<Product>>.Success(ProductsPaged);
    }

    public async Task<IServiceResponse<IEnumerable<Product>>> GetAllProductsSorted()
    {
        var products = await _unitOfWork.Product.GetAllSortedAsync(m => m.Category, GeneralConstants.OrderBy.Ascending, x => x.Category, x => x.Supplier);
        if (products.Any() is false)
            return ServiceResponse<IEnumerable<Product>>.Fail("Products not Found", StatusCodes.Status404NotFound);

        return ServiceResponse<IEnumerable<Product>>.Success(products);
    }

    public async Task<IServiceResponse<IEnumerable<Product>>> GetProductsFilterdAndSorted()
    {
        var products = await _unitOfWork.Product.GetFilteredSortedAsync(m => m.Price >= 50, m => m.Category, GeneralConstants.OrderBy.Ascending,
            x => x.Category, x => x.Supplier);

        if (products.Any() is false)
            return ServiceResponse<IEnumerable<Product>>.Fail("Products not Found", StatusCodes.Status404NotFound);

        return ServiceResponse<IEnumerable<Product>>.Success(products);
    }

    public async Task<IServiceResponse<int>> GetProductsCounts()
    {
        return ServiceResponse<int>.Success(await _unitOfWork.Product.CountAsync());
    }

    public async Task<IServiceResponse<decimal>> GetProductsMax()
    {
        return ServiceResponse<decimal>.Success(await _unitOfWork.Product.MaxAsync(m => m.Price));
    }

    public async Task<IServiceResponse<decimal>> GetProductsMin()
    {
        return ServiceResponse<decimal>.Success(await _unitOfWork.Product.MinAsync(m => m.Price));
    }

    public async Task<IServiceResponse<decimal>> GetProductsSum()
    {
        return ServiceResponse<decimal>.Success(await _unitOfWork.Product.SumAsync(m => m.Price));
    }

    public async Task<IServiceResponse<decimal>> GetProductsAverage()
    {
        return ServiceResponse<decimal>.Success(await _unitOfWork.Product.AverageAsync(m => m.Price));
    }
}