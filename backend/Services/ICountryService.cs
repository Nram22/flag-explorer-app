// Services/ICountryService.cs
using backend.Models;

namespace backend.Services
{
    // Service interface for retrieving country data
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task<CountryDetails?> GetCountryByNameAsync(string name);
    }
}
