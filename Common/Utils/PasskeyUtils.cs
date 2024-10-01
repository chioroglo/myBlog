namespace Common.Utils;

public static class PasskeyUtils
{
    public static string RegistrationCacheKey(int userId) => $"registration-session_{userId}";
    public static string AuthenticationCacheKey(string challenge) => $"authentication-session_{challenge}";
}