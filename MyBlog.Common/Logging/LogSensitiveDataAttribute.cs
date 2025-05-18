using Destructurama.Attributed;

namespace MyBlog.Common.Logging;

public class LogSensitiveDataAttribute : LogMaskedAttribute
{
    public LogSensitiveDataAttribute()
    {
        Text = "***REDACTED***";
    }
}