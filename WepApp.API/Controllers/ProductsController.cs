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
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.StatusCode, result.Message));

        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDTO productDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _productService.AddAsync(productDTO);

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.StatusCode, result.Message));

        return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, [FromBody] ProductDTO productDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _productService.UpdateAsync(id, productDTO);

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.StatusCode, result.Message));

        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteAsync(id);

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.StatusCode, result.Message));

        return NoContent();
    }

    [HttpGet("GetProductData/{id}", Name = "GetProductData")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var result = await _productService.GetProductData(id);

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.StatusCode, result.Message));

        return Ok(result.Data);
    }

    [HttpGet("GetAllPaged")]
    public async Task<IActionResult> GetAllPaged(int pageNumber, int pageSize)
    {
        var result = await _productService.GetProductsWithPages(pageNumber, pageSize);

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.StatusCode, result.Message));

        return Ok(result.Data);
    }

    [HttpGet("GetProductsFilterd")]
    public async Task<IActionResult> GetProductsFilterd()
    {
        var result = await _productService.GetProductsFilterd();

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.StatusCode, result.Message));

        return Ok(result.Data);
    }

    [HttpGet("GetProductsFilteredPaged")]
    public async Task<IActionResult> GetProductsFilteredPaged(int pageNumber, int pageSize)
    {
        var result = await _productService.GetProductsFilterdWithPages(pageNumber, pageSize);

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.StatusCode, result.Message));

        return Ok(result.Data);
    }

    [HttpGet("GetAllProductsSorted")]
    public async Task<IActionResult> GetAllProductsSorted()
    {
        var result = await _productService.GetAllProductsSorted();

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.StatusCode, result.Message));

        return Ok(result.Data);
    }

    [HttpGet("GetProductsSortedAndFiltered")]
    public async Task<IActionResult> GetProductsSortedAndFiltered()
    {
        var result = await _productService.GetProductsFilterdAndSorted();

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.StatusCode, result.Message));

        return Ok(result.Data);
    }

    [HttpGet("Count")]
    public async Task<IActionResult> Count()
    {
        var result = await _productService.GetProductsCounts();

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.StatusCode, result.Message));

        return Ok(result.Data);
    }

    [HttpGet("Max")]
    public async Task<IActionResult> Max()
    {
        var result = await _productService.GetProductsMax();

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.StatusCode, result.Message));

        return Ok(result.Data);
    }

    [HttpGet("Min")]
    public async Task<IActionResult> Min()
    {
        var result = await _productService.GetProductsMin();

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.StatusCode, result.Message));

        return Ok(result.Data);
    }

    [HttpGet("Sum")]
    public async Task<IActionResult> Sum()
    {
        var result = await _productService.GetProductsSum();

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.StatusCode, result.Message));

        return Ok(result.Data);
    }

    [HttpGet("Average")]
    public async Task<IActionResult> Average()
    {
        var result = await _productService.GetProductsAverage();

        if (result.IsSuccess is false)
            return StatusCode(result.StatusCode, ProblemFactory.CreateProblemDetails(HttpContext, result.StatusCode, result.Message));

        return Ok(result.Data);
    }
}