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

    //--------------------------------------------------------------------------------
    // Clock
    //--------------------------------------------------------------------------------

    public async Task<ClockResult> GetClockInfoAsync()
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/GetClockInfo"
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ClockResult>(json)!;
    }

    public async Task<Result> SelectClockIdAsync(int id)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/SetClockSelectId",
            ClockId = id
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    //--------------------------------------------------------------------------------
    // Cloud
    //--------------------------------------------------------------------------------

    public async Task<Result> SelectCloudIndexAsync(CloudIndex index)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/CloudIndex",
            CustomPageIndex = (int)index
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    //--------------------------------------------------------------------------------
    // Equalizer
    //--------------------------------------------------------------------------------

    public async Task<Result> SelectEqualizerIdAsync(int index)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/SetEqPosition",
            EqPosition = index
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    //--------------------------------------------------------------------------------
    // Custom
    //--------------------------------------------------------------------------------

    public async Task<Result> SelectCustomPageAsync(int index)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/SetCustomPageIndex",
            CustomPageIndex = index
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    //--------------------------------------------------------------------------------
    // Tool
    //--------------------------------------------------------------------------------

    public async Task<Result> TimerToolAsync(bool enable, int second)
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

    public async Task<Result> StopwatchToolAsync(StopwatchCommand command)
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

    public async Task<Result> ScoreboardToolAsync(int blue, int red)
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

    public async Task<Result> NoiseToolAsync(bool enable)
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
    // Setting
    //--------------------------------------------------------------------------------

    public async Task<TimeResult> GetDeviceTimeAsync()
    {
        using var request = CreateRequest(new
        {
            Command = "Device/GetDeviceTime"
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TimeResult>(json)!;
    }

    public async Task<WeatherResult> GetWeatherInfoAsync()
    {
        using var request = CreateRequest(new
        {
            Command = "Device/GetWeatherInfo"
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<WeatherResult>(json)!;
    }

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
            Command = "Channel/SetBrightness",
            Brightness = brightness
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    public async Task<Result> SetScreenRotationAsync(RotationAngle rotation)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetScreenRotationAngle",
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

    public async Task<Result> SetWhiteBalanceAsync(int r, int g, int b)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetWhiteBalance",
            RValue = r,
            GValue = g,
            BValue = b
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    public async Task<ConfigResult> GetAllConfigAsync()
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/GetAllConf"
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ConfigResult>(json)!;
    }

    public async Task<Result> ConfigLogAndLatAsync(double lon, double lat)
    {
        using var request = CreateRequest(new
        {
            Command = "Sys/LogAndLat",
            Longitude = lon,
            Latitude = lat
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    public async Task<Result> ConfigTimeZoneAsync(string tz)
    {
        using var request = CreateRequest(new
        {
            Command = "Sys/TimeZone",
            TimeZoneValue = tz
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    public async Task<Result> ConfigSystemTimeAsync(DateTimeOffset utc)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetUTC",
            Utc = utc.ToUnixTimeSeconds()
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    public async Task<Result> ConfigTemperatureModeAsync(TemperatureMode mode)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetDisTempMode",
            Mode = (int)mode
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    public async Task<Result> ConfigHourModeAsync(HourMode mode)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetTime24Flag",
            Mode = (int)mode
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    //--------------------------------------------------------------------------------
    // Text
    //--------------------------------------------------------------------------------

    // TODO

    public async Task<Result> ClearTextAsync()
    {
        using var request = CreateRequest(new
        {
            Command = "Draw/ClearHttpText"
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Result>(json)!;
    }

    //--------------------------------------------------------------------------------
    // Image
    //--------------------------------------------------------------------------------

    // TODO
}
