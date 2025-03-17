public class CountryApiResponse
{
    public NameProperty name { get; set; } = new NameProperty();
    public FlagsProperty flags { get; set; } = new FlagsProperty();
    public List<string>? capital { get; set; } = new List<string>();
    public int? population { get; set; }

    public class NameProperty
    {
        public string common { get; set; } = string.Empty;
    }

    public class FlagsProperty
    {
        public string png { get; set; } = string.Empty;
    }
}
