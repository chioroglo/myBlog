namespace Common.Options
{
    public class JsonWebTokenOptions : BaseApplicationOptions
    {
        public static string Config => "Jwt";
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ValidityTimeMinutes { get; set; }
    }
}