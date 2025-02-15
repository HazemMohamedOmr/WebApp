using Microsoft.AspNetCore.Http;
using WebApp.Core.DTOs;
using WebApp.Core.Interfaces;
using WebApp.Core.Models;
using WebApp.Core.Repositories;
using WebApp.Core.Services;
using WebApp.Infrastructure.Exceptions;

namespace WebApp.Service;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IServiceResponse<IEnumerable<Category>>> GetAllAsync()
    {
        var categories = await _unitOfWork.Category.GetAllAsync();
        return ServiceResponse<IEnumerable<Category>>.Success(categories);
    }

    public async Task<IServiceResponse<Category>> GetByIdAsync(int id)
    {
        var category = await _unitOfWork.Category.GetByIdAsync(id);
        if (category is null)
            return ServiceResponse<Category>.Fail("Category not Found", StatusCodes.Status404NotFound);
        return ServiceResponse<Category>.Success(category);
    }

    public async Task<IServiceResponse<Category>> AddAsync(CategoryDTO categoryDto)
    {
        var category = new Category()
        {
            Name = categoryDto.Name.Trim(),
            Description = categoryDto.Description?.Trim(),
        };
        var createdCategory = await _unitOfWork.Category.AddAsync(category);
        await _unitOfWork.SaveAsync();
        return ServiceResponse<Category>.Success(createdCategory.Entity);
    }

    public async Task<IServiceResponse<Category>> UpdateAsync(int id, CategoryDTO categoryDto)
    {
        var category = await _unitOfWork.Category.GetByIdAsync(id);
        if (category is null)
            return ServiceResponse<Category>.Fail("Category not Found", StatusCodes.Status404NotFound);

        category.Description = categoryDto.Description?.Trim();
        category.Name = categoryDto.Name.Trim();

        await _unitOfWork.SaveAsync();
        return ServiceResponse<Category>.Success(category);
    }

    public async Task<IServiceResponse<bool>> DeleteAsync(int id)
    {
        var category = await _unitOfWork.Category.GetByIdAsync(id).ConfigureAwait(false);
        if (category is null)
            return ServiceResponse<bool>.Fail("Category not Found", StatusCodes.Status404NotFound);
        _unitOfWork.Category.Delete(category);
        await _unitOfWork.SaveAsync();
        return ServiceResponse<bool>.Success(true);
    }
}