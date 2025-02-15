using Microsoft.AspNetCore.Mvc;
using WebApp.Core.DTOs;
using WebApp.Core.Models;
using WebApp.Core.Repositories;
using WebApp.Core.Services;
using WebApp.Infrastructure.Exceptions;
using WebApp.Service;

namespace WebApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _productService.GetAllAsync();
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _productService.GetByIdAsync(id);

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.Message));

        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDTO productDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _productService.AddAsync(productDTO);

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.Message));

        return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, [FromBody] ProductDTO productDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _productService.UpdateAsync(id, productDTO);

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.Message));

        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteAsync(id);

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.Message));

        return NoContent();
    }
}