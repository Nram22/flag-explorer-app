// Models/CountryDetails.cs
namespace backend.Models
{
    // Represents detailed information about a country (name, capital, population, flag URL)
    public class CountryDetails
    {
        public required string Name { get; set; }
        public required string Capital { get; set; }
        public int Population { get; set; }
        public required string Flag { get; set; }
    }
}
