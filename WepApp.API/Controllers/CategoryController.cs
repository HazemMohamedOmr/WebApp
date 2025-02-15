using Microsoft.AspNetCore.Mvc;
using WebApp.Core.DTOs;
using WebApp.Core.Models;
using WebApp.Core.Repositories;
using WebApp.Core.Services;
using WebApp.Infrastructure.Exceptions;

namespace WebApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _categoryService.GetAllAsync();
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _categoryService.GetByIdAsync(id);

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.Message));

        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryDTO categoryDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _categoryService.AddAsync(categoryDto);

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.Message));

        return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, CategoryDTO categoryDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _categoryService.UpdateAsync(id, categoryDto);

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.Message));

        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _categoryService.DeleteAsync(id);

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.Message));

        return NoContent();
    }
}