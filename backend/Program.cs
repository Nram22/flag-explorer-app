// backend/Program.cs
using backend.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add controllers for API endpoints.
builder.Services.AddControllers();

// Configure CORS to allow requests from the React app running on http://localhost:3000.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Register memory cache and the country service.
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICountryService, CountryService>();

// Add Swagger for API documentation.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Country API", Version = "v1" });
});

var app = builder.Build();

// Prewarm the cache (optional: run as a background task during startup).
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<ICountryService>() as CountryService;
    if (service != null)
    {
        // For simplicity, synchronously wait here. In production, consider an asynchronous startup task.
        service.PrewarmCacheAsync().Wait();
    }
}

app.UseCors("AllowReactApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
