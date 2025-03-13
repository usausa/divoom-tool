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

    //--------------------------------------------------------------------------------
    // Channel
    //--------------------------------------------------------------------------------

    public async Task<IndexResult> GetChannelIndexAsync()
    {
        using var request = CreateRequest(new DeviceRequest
        {
            Command = "Channel/GetIndex"
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IndexResult>(json)!;
    }

    public async Task<Result> SetChannelIndexAsync(Channel channel)
    {
        using var request = CreateRequest(new IndexRequest
        {
            Command = "Channel/SetIndex",
            Index = (int)channel
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    // TODO

    //--------------------------------------------------------------------------------
    // Tool
    //--------------------------------------------------------------------------------

    public async Task<Result> SetTimerAsync(bool enable, int second)
    {
        using var request = CreateRequest(new TimerRequest
        {
            Command = "Tools/SetTimer",
            Minute = second / 60,
            Second = second % 60,
            Status = enable ? 1 : 0
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    public async Task<Result> SetStopWatchAsync(StopWatchCommand command)
    {
        using var request = CreateRequest(new StopWatchRequest
        {
            Command = "Tools/SetStopWatch",
            Status = (int)command
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    // TODO

    //--------------------------------------------------------------------------------
    // Control
    //--------------------------------------------------------------------------------

    public async Task<Result> SwitchScreenAsync(bool on)
    {
        using var request = CreateRequest(new ScreenRequest
        {
            Command = "Channel/OnOffScreen",
            OnOff = on ? 1 : 0
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    // TODO

    //--------------------------------------------------------------------------------
    // Config
    //--------------------------------------------------------------------------------

    // TODO
}
