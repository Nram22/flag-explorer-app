// backend/Tests/CountryServiceTests.cs
using backend.Models;
using backend.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace backend.Tests
{
    public class CountryServiceTests
    {
        private readonly IMemoryCache _memoryCache;

        public CountryServiceTests()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        [Fact]
        public async Task GetAllCountriesAsync_ReturnsAlphabeticallySortedCountries()
        {
            // Arrange: Create dummy unsorted data.
            var dummyData = new List<CountryApiResponse>
            {
                new CountryApiResponse 
                { 
                    name = new CountryApiResponse.NameProperty { common = "Germany" },
                    flags = new CountryApiResponse.FlagsProperty { png = "url_de" } 
                },
                new CountryApiResponse 
                { 
                    name = new CountryApiResponse.NameProperty { common = "France" },
                    flags = new CountryApiResponse.FlagsProperty { png = "url_fr" } 
                },
                new CountryApiResponse 
                { 
                    name = new CountryApiResponse.NameProperty { common = "Canada" },
                    flags = new CountryApiResponse.FlagsProperty { png = "url_ca" } 
                }
            };

            // Manually set the dummy data in the cache.
            _memoryCache.Set("AllCountries", dummyData);

            var service = new CountryService(_memoryCache);

            // Act: Retrieve the list of countries.
            var countries = await service.GetAllCountriesAsync();
            var countryList = countries.ToList();

            // Assert: Verify the list is sorted alphabetically.
            Assert.Equal("Canada", countryList[0].Name);
            Assert.Equal("France", countryList[1].Name);
            Assert.Equal("Germany", countryList[2].Name);
        }

        [Fact]
        public async Task PrewarmCacheAsync_PopulatesCache()
        {
            // Arrange: Ensure cache is cleared.
            _memoryCache.Remove("AllCountries");
            var service = new CountryService(_memoryCache);

            // Act: Prewarm the cache.
            await service.PrewarmCacheAsync();
            var cachedData = _memoryCache.Get<List<CountryApiResponse>>("AllCountries");

            // Assert: The cache should be populated.
            Assert.NotNull(cachedData);
            // Optionally, check that there is at least one country in the cache.
            Assert.True(cachedData.Count > 0);
        }
    }
}
