namespace Common.Options
{
    public class JsonWebTokenOptions
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ValidityTimeMinutes { get; set; }
    }
}