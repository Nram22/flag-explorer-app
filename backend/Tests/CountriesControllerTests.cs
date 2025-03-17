// Tests/CountriesControllerTests.cs
using backend.Controllers;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Tests
{
    public class CountriesControllerTests
    {
        private readonly Mock<ICountryService> _mockService;
        private readonly CountriesController _controller;

        // Constructor to initialize mock service and controller
        public CountriesControllerTests()
        {
            _mockService = new Mock<ICountryService>(); // Create a mock instance of ICountryService
            _controller = new CountriesController(_mockService.Object); // Inject mock into the controller
        }

        [Fact]
        public async Task GetCountries_ReturnsOkResult_WithListOfCountries()
        {
            // Arrange: Prepare a mock list of countries
            var mockCountries = new List<Country>
            {
                new Country { Name = "France", Flag = "https://flagcdn.com/fr.png" },
                new Country { Name = "Germany", Flag = "https://flagcdn.com/de.png" }
            };

            _mockService.Setup(s => s.GetAllCountriesAsync()).ReturnsAsync(mockCountries);

            // Act: Call the controller method
            var result = await _controller.GetCountries();

            // Assert: Verify the response is OK and contains expected data
            var okResult = Assert.IsType<OkObjectResult>(result); // Check if the result is 200 OK
            var countries = Assert.IsAssignableFrom<IEnumerable<Country>>(okResult.Value); // Verify data type
            Assert.NotNull(countries); // Ensure it's not null
            Assert.NotEmpty(countries); // Ensure the list is not empty
            Assert.Equal(2, countries.Count()); // Validate the number of returned items

            // Verify that the mock service method was called exactly once
            _mockService.Verify(s => s.GetAllCountriesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetCountryByName_WhenCountryExists_ReturnsOkResult_WithCountryDetails()
        {
            // Arrange: Prepare mock country details
            var details = new CountryDetails
            {
                Name = "France",
                Capital = "Paris",
                Population = 67000000,
                Flag = "https://flagcdn.com/fr.png"
            };

            _mockService.Setup(s => s.GetCountryByNameAsync("France")).ReturnsAsync(details);

            // Act: Call the controller method
            var result = await _controller.GetCountryByName("France");

            // Assert: Verify the response is OK and contains expected details
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultDetails = Assert.IsType<CountryDetails>(okResult.Value);

            Assert.NotNull(resultDetails);
            Assert.Equal("France", resultDetails.Name);
            Assert.Equal("Paris", resultDetails.Capital);
            Assert.Equal(67000000, resultDetails.Population);
            Assert.Equal("https://flagcdn.com/fr.png", resultDetails.Flag);

            // Ensure the service method was called with correct parameters
            _mockService.Verify(s => s.GetCountryByNameAsync("France"), Times.Once);
        }

        [Fact]
        public async Task GetCountryByName_WhenCountryDoesNotExist_ReturnsNotFound()
        {
            // Arrange: Mock service to return null for a non-existent country
            _mockService.Setup(s => s.GetCountryByNameAsync(It.IsAny<string>())).ReturnsAsync((CountryDetails?)null);

            // Act: Call the controller method
            var result = await _controller.GetCountryByName("NonExistent");

            // Assert: Verify response is NotFound (404)
            Assert.IsType<NotFoundResult>(result);

            // Verify that the service method was called with correct parameters
            _mockService.Verify(s => s.GetCountryByNameAsync("NonExistent"), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetCountryByName_WithInvalidInput_ReturnsBadRequest(string? name)
        {
            // Act: Call the method with invalid input
            var result = await _controller.GetCountryByName(name);

            // Assert: Ensure the response is BadRequest (400)
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
