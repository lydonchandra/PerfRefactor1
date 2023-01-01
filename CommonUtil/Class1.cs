using System.Text.Json;

namespace CommonUtil;

public static class Dumper
{
    public static string ToPrettyString(this object value)
    {
        return JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = true });
    }

    public static T Dump<T>(this T value)
    {
        Console.WriteLine(value.ToPrettyString());
        return value;
    }
}