using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;
        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        // GET /api/countries - Retrieves a list of countries (name and flag URL)
        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _countryService.GetAllCountriesAsync();
            return Ok(countries);
        }

        // GET /api/countries/{name} - Retrieves detailed info for a specific country
[HttpGet("{name}")]
public async Task<IActionResult> GetCountryByName(string? name) // nullable name
{
    if (string.IsNullOrWhiteSpace(name))
        return BadRequest("Country name cannot be empty.");

    var country = await _countryService.GetCountryByNameAsync(name);

    if (country == null)
        return NotFound(new { message = $"Country '{name}' not found." });

    return Ok(country);
}

    }
}