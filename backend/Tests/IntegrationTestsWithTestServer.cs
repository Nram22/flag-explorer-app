// backend/Tests/IntegrationTestsWithTestServer.cs
using System.Net;
using System.Net.Http.Json;
using backend.Models;
using backend.Controllers; // For accessing CountriesController type
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace backend.Tests
{
    public class IntegrationTestsWithTestServer
    {
        private readonly HttpClient _client;

        public IntegrationTestsWithTestServer()
        {
            // Create the host using minimal API style and ensure controllers are discovered.
            var host = CreateHost();
            _client = host.GetTestClient();
        }

        private static IHost CreateHost()
        {
            // Create a builder with specified options.
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                EnvironmentName = "Development"
            });

            // Use TestServer for an in-memory host.
            builder.WebHost.UseTestServer();

            // Add controllers explicitly using ApplicationPart so that controllers in the backend assembly are discovered.
            builder.Services.AddControllers().AddApplicationPart(typeof(CountriesController).Assembly);

            // Register other services as in your Program.cs.
            builder.Services.AddScoped<backend.Services.ICountryService, backend.Services.CountryService>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            builder.Services.AddMemoryCache();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Country API",
                    Version = "v1"
                });
            });

            // Build and configure the app.
            var app = builder.Build();
            app.UseCors("AllowReactApp");
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Start the test server.
            app.StartAsync().Wait();
            return app;
        }

        [Fact]
        public async Task GetCountries_ReturnsOkAndData()
        {
            // Act: Call GET /api/countries
            var response = await _client.GetAsync("/api/countries");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var countries = await response.Content.ReadFromJsonAsync<IEnumerable<Country>>();
            Assert.NotNull(countries);
            Assert.NotEmpty(countries);
        }

        [Fact]
        public async Task GetCountryByName_ReturnsOk_WhenCountryExists()
        {
            // Act: Call GET /api/countries/France
            var response = await _client.GetAsync("/api/countries/France");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var details = await response.Content.ReadFromJsonAsync<CountryDetails>();
            Assert.NotNull(details);
            Assert.Equal("France", details.Name);
            Assert.Equal("Paris", details.Capital);
        }

        [Fact]
        public async Task GetCountryByName_ReturnsNotFound_WhenCountryDoesNotExist()
        {
            // Act: Call GET /api/countries/NonExistent
            var response = await _client.GetAsync("/api/countries/NonExistent");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
