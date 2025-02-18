using System.Text.Json;

namespace MyBlog.Common.Utils;

public static class CopyUtils
{
    public static T? DeepCopyJson<T>(T input) where T : class, new()
    {
        var str = JsonSerializer.Serialize(input);
        return JsonSerializer.Deserialize<T>(str, JsonSerializerOptions.Default);
    }
}