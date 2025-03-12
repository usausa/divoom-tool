using System.Net.Http.Json;

namespace Divoom.Client;

using System.Text.Json;

#pragma warning disable CA2234
public sealed class DeviceClient : IDisposable
{
    private const string PostUrl = "post";

    private readonly HttpClient client = new();

    public DeviceClient(string host)
    {
        client.BaseAddress = new Uri($"http://{host}:80");
    }

    public void Dispose()
    {
        client.Dispose();
    }

    private static StringContent CreateRequest(object request) =>
        new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

    public async Task<CurrentChannelResult> GetCurrentChannelAsync()
    {
        using var request = CreateRequest(new DeviceRequest
        {
            Command = "Channel/GetIndex"
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CurrentChannelResult>(json)!;
    }
}
