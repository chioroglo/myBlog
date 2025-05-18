using Destructurama.Attributed;

namespace MyBlog.Common.Logging;

public class LogPersonalDataAttribute : LogMaskedAttribute
{
    public LogPersonalDataAttribute()
    {
        PreserveLength = true;
        ShowFirst = 2;
        ShowLast = 0;
    }
}