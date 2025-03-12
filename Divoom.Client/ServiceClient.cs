namespace Divoom.Client;

using System.Text.Json;

#pragma warning disable CA2234
public sealed class ServiceClient : IDisposable
{
    private readonly HttpClient client = new()
    {
        BaseAddress = new Uri("http://app.divoom-gz.com")
    };

    public void Dispose()
    {
        client.Dispose();
    }

    public async Task<DeviceListResult> GetDeviceListAsync()
    {
        var response = await client.GetAsync("Device/ReturnSameLANDevice").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceListResult>(json)!;
    }
}
