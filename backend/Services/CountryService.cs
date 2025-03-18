// backend/Services/CountryService.cs
using backend.Models;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace backend.Services
{
    // Implements the country service using an external API with in-memory caching.
    public class CountryService : ICountryService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "AllCountries";
        private const string BaseUrl = "https://restcountries.com/v3.1/all";

        public CountryService(IMemoryCache cache)
        {
            _httpClient = new HttpClient();
            _cache = cache;
        }

        // Retrieves all countries, maps to the Country model, and sorts them alphabetically.
        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            var data = await GetCachedDataAsync();
            return data
                .Select(item => new Country
                {
                    Name = item.name.common,
                    Flag = item.flags.png
                })
                .OrderBy(c => c.Name);
        }

        // Retrieves detailed information for a specified country.
        public async Task<CountryDetails?> GetCountryByNameAsync(string name)
        {
            var data = await GetCachedDataAsync();
            var item = data.FirstOrDefault(c =>
                string.Equals(c.name.common, name, System.StringComparison.OrdinalIgnoreCase));

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

        // Retrieves the data from cache, or fetches from the external API if not cached.
        private async Task<List<CountryApiResponse>> GetCachedDataAsync()
        {
            if (!_cache.TryGetValue(CacheKey, out List<CountryApiResponse> cachedData))
            {
                cachedData = await _httpClient.GetFromJsonAsync<List<CountryApiResponse>>(BaseUrl)
                             ?? new List<CountryApiResponse>();

                // Set cache options (e.g., sliding expiration of 1 hour)
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(1));

                _cache.Set(CacheKey, cachedData, cacheEntryOptions);
            }
            return cachedData;
        }

        // Prewarm the cache by fetching the data at startup.
        public async Task PrewarmCacheAsync()
        {
            await GetCachedDataAsync();
        }
    }
}
