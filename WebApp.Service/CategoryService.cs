using WebApp.Core.DTOs;
using WebApp.Core.Models;
using WebApp.Core.Repositories;
using WebApp.Core.Services;

namespace WebApp.Service;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _unitOfWork.Category.GetAllAsync();
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        return await _unitOfWork.Category.GetByIdAsync(id);
    }

    public async Task<Category> AddAsync(CategoryDTO categoryDto)
    {
        var category = new Category()
        {
            Name = categoryDto.Name.Trim(),
            Description = categoryDto.Description?.Trim(),
        };
        var createdCategory = await _unitOfWork.Category.AddAsync(category);
        await _unitOfWork.SaveAsync();
        return createdCategory.Entity;
    }

    public async Task<Category> UpdateAsync(int id, CategoryDTO categoryDto)
    {
        var category = await _unitOfWork.Category.GetByIdAsync(id);
        if (category is null)
            return category;

        category.Description = categoryDto.Description?.Trim();
        category.Name = categoryDto.Name.Trim();

        await _unitOfWork.SaveAsync();
        return category;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _unitOfWork.Category.GetByIdAsync(id).ConfigureAwait(false);
        if (category is null)
            return false;
        _unitOfWork.Category.Delete(category);
        await _unitOfWork.SaveAsync();
        return true;
    }
}