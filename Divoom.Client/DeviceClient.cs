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
        using var request = CreateRequest(new
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
        using var request = CreateRequest(new
        {
            Command = "Channel/SetIndex",
            SelectIndex = (int)channel
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
        using var request = CreateRequest(new
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

    public async Task<Result> SetStopwatchAsync(StopwatchCommand command)
    {
        using var request = CreateRequest(new
        {
            Command = "Tools/SetStopwatch",
            Status = (int)command
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    public async Task<Result> SetScoreboardAsync(int blue, int red)
    {
        using var request = CreateRequest(new
        {
            Command = "Tools/SetScoreBoard",
            BlueScore = blue,
            RedScore = red
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    public async Task<Result> SetNoiseStatusAsync(bool enable)
    {
        using var request = CreateRequest(new
        {
            Command = "Tools/SetNoiseStatus",
            NoiseStatus = enable ? 1 : 0
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    //--------------------------------------------------------------------------------
    // Control
    //--------------------------------------------------------------------------------

    public async Task<Result> PlayBuzzerAsync(int active, int off, int total)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/PlayBuzzer",
            ActiveTimeInCycle = active,
            OffTimeInCycle = off,
            PlayTotalTime = total
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    public async Task<Result> SwitchScreenAsync(bool on)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/OnOffScreen",
            OnOff = on ? 1 : 0
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    public async Task<Result> SetBrightnessAsync(int brightness)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetBrightness",
            Brightness = brightness
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    public async Task<Result> SetScreenRotationAngleAsync(RotationAngle rotation)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetMirrorMode",
            Mode = (int)rotation
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    public async Task<Result> SetMirrorModeAsync(bool on)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetMirrorMode",
            Mode = on ? 1 : 0
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    public async Task<Result> SetHighlightModeAsync(bool on)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetHighLightMode",
            Mode = on ? 1 : 0
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
