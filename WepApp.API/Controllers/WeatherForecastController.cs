using Microsoft.AspNetCore.Mvc;
using WebApp.Core.Repositories;

namespace WepApp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    //[HttpGet]
    //public async Task<IActionResult> whatevermethod()
    //{
    //    var x = await _unitOfWork.Product.GetAllAsync(m => m.Category, p => p.Supplier);
    //    //var y = await _unitOfWork.Category
    //    return NotFound();
    //}
}