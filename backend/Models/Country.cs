// Models/Country.cs
namespace backend.Models
{
    // Represents a summary view of a country (name and flag URL)
    public class Country
    {
        public required string Name { get; set; }
        public required string Flag { get; set; }
    }
}
