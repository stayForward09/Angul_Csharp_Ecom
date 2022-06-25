using System.Text.Json;

namespace StackApi.Extensions;

public static class HttpClientExtensions
{
    public static async Task<T> readContentAs<T>(this HttpResponseMessage mg)
    {
        var data = await mg.Content.ReadAsStringAsync().ConfigureAwait(false);
        return JsonSerializer.Deserialize<T>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}