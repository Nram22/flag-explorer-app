using backend.Models;
using System.Net.Http.Json;

namespace backend.Services
{
    // Implements the country service using an external API
    public class CountryService : ICountryService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://restcountries.com/v3.1/all";
      private List<CountryApiResponse> _cachedData = new List<CountryApiResponse>(); // ✅ Use strongly-typed model
        public CountryService()
        {
            _httpClient = new HttpClient();
        }

        // Retrieves all countries and maps them to the Country model
     public async Task<IEnumerable<Country>> GetAllCountriesAsync()
{
    await EnsureDataLoadedAsync();
    return _cachedData.Select(item => new Country
    {
        Name = item.name.common, //
        Flag = item.flags.png    // 
    });
}


        // Retrieves detailed info for a specified country
public async Task<CountryDetails?> GetCountryByNameAsync(string name)
{
    await EnsureDataLoadedAsync();
    var item = _cachedData.FirstOrDefault(c =>
        string.Equals(c.name.common, name, StringComparison.OrdinalIgnoreCase));

    if (item == null)
        return null;

    return new CountryDetails
    {
        Name = item.name.common,
        Flag = item.flags.png,
        Capital = item.capital != null && item.capital.Any() ? item.capital[0] : "N/A",
        Population = item.population ?? 0
    };
}


        // Loads data from the external API if not already cached
      private async Task EnsureDataLoadedAsync()
{
    if (_cachedData.Count == 0) // Ensures data is only loaded once
    {
        var response = await _httpClient.GetFromJsonAsync<List<CountryApiResponse>>(BaseUrl);
        _cachedData = response ?? new List<CountryApiResponse>(); // ✅ Strongly typed list
    }
}
    }
}
