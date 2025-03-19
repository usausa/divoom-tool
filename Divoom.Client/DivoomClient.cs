namespace Divoom.Client;

using System;
using System.Text.Json;

#pragma warning disable CA2234
public sealed class DivoomClient : IDisposable
{
    private const string PostUrl = "post";

    private readonly HttpClient client = new();

    public DivoomClient(string host)
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
    // Service
    //--------------------------------------------------------------------------------

    private static HttpClient CreateServiceClient() => new() { BaseAddress = new("http://app.divoom-gz.com") };

    public static async Task<DeviceListResult> GetDeviceListAsync()
    {
        using var client = CreateServiceClient();
        var response = await client.GetAsync("Device/ReturnSameLANDevice").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceListResult>(json)!;
    }

    public static async Task<FontListResult> GetFontListAsync()
    {
        using var client = CreateServiceClient();
        var response = await client.GetAsync("Device/GetTimeDialFontList").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<FontListResult>(json)!;
    }

    public static async Task<DialTypeResult> GetDialTypeAsync()
    {
        using var client = CreateServiceClient();
        var response = await client.GetAsync("Channel/GetDialType").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DialTypeResult>(json)!;
    }

    public static async Task<DialListResult> GetDialListAsync(string dial, string? device, int page)
    {
        using var client = CreateServiceClient();
        using var request = CreateRequest(new
        {
            DialType = dial,
            DeviceType = device,
            Page = page
        });
        var response = await client.PostAsync("Channel/GetDialList", request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DialListResult>(json)!;
    }

    public static async Task<ImageListResult> GetUploadImageListAsync(int deviceId, string mac, int page = 1)
    {
        using var client = CreateServiceClient();
        using var request = CreateRequest(new
        {
            DeviceId = deviceId,
            DeviceMac = mac,
            Page = page
        });
        var response = await client.PostAsync("Device/GetImgUploadList", request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ImageListResult>(json)!;
    }

    public static async Task<ImageListResult> GetLikeImageListAsync(int deviceId, string mac, int page = 1)
    {
        using var client = CreateServiceClient();
        using var request = CreateRequest(new
        {
            DeviceId = deviceId,
            DeviceMac = mac,
            Page = page
        });
        var response = await client.PostAsync("Device/GetImgLikeList", request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ImageListResult>(json)!;
    }

    //--------------------------------------------------------------------------------
    // Reboot
    //--------------------------------------------------------------------------------

    public async Task<IndexResult> RebootAsync()
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SysReboot"
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IndexResult>(json)!;
    }

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

    public async Task<DeviceResult> SetChannelIndexAsync(Channel channel)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/SetIndex",
            SelectIndex = (int)channel
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
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

    public async Task<DeviceResult> SelectClockIdAsync(int id)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/SetClockSelectId",
            ClockId = id
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    //--------------------------------------------------------------------------------
    // Cloud
    //--------------------------------------------------------------------------------

    public async Task<DeviceResult> SelectCloudIndexAsync(CloudIndex index)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/CloudIndex",
            CustomPageIndex = (int)index
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    //--------------------------------------------------------------------------------
    // Equalizer
    //--------------------------------------------------------------------------------

    public async Task<DeviceResult> SelectEqualizerIdAsync(int index)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/SetEqPosition",
            EqPosition = index
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    //--------------------------------------------------------------------------------
    // Custom
    //--------------------------------------------------------------------------------

    public async Task<DeviceResult> SelectCustomPageAsync(int index)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/SetCustomPageIndex",
            CustomPageIndex = index
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    //--------------------------------------------------------------------------------
    // Monitor
    //--------------------------------------------------------------------------------

    public async Task<DeviceResult> UpdatePcMonitorAsync(IEnumerable<MonitorParameter> parameters)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/UpdatePCParaInfo",
            ScreenList = parameters.Select(static x => new
            {
                LcdId = default(string),
                DispData = new[] { x.CpuUsed, x.GpuUsed, x.CpuTemperature, x.GpuTemperature, x.MemoryUsed, x.DiskTemperature }
            })
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    //--------------------------------------------------------------------------------
    // Tool
    //--------------------------------------------------------------------------------

    public async Task<DeviceResult> TimerToolAsync(bool enable, int second)
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
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    public async Task<DeviceResult> StopwatchToolAsync(StopwatchCommand command)
    {
        using var request = CreateRequest(new
        {
            Command = "Tools/SetStopWatch",
            Status = (int)command
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    public async Task<DeviceResult> ScoreboardToolAsync(int blue, int red)
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
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    public async Task<DeviceResult> NoiseToolAsync(bool enable)
    {
        using var request = CreateRequest(new
        {
            Command = "Tools/SetNoiseStatus",
            NoiseStatus = enable ? 1 : 0
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
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

    public async Task<DeviceResult> PlayBuzzerAsync(int active, int off, int total)
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
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    public async Task<DeviceResult> SwitchScreenAsync(bool on)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/OnOffScreen",
            OnOff = on ? 1 : 0
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    public async Task<DeviceResult> SetBrightnessAsync(int brightness)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/SetBrightness",
            Brightness = brightness
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    public async Task<DeviceResult> SetScreenRotationAsync(RotationAngle rotation)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetScreenRotationAngle",
            Mode = (int)rotation
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    public async Task<DeviceResult> SetMirrorModeAsync(bool on)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetMirrorMode",
            Mode = on ? 1 : 0
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    public async Task<DeviceResult> SetHighlightModeAsync(bool on)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetHighLightMode",
            Mode = on ? 1 : 0
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    public async Task<DeviceResult> SetWhiteBalanceAsync(int r, int g, int b)
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
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
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

    public async Task<DeviceResult> ConfigLogAndLatAsync(double lon, double lat)
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
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    public async Task<DeviceResult> ConfigTimeZoneAsync(string tz)
    {
        using var request = CreateRequest(new
        {
            Command = "Sys/TimeZone",
            TimeZoneValue = tz
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    public async Task<DeviceResult> ConfigSystemTimeAsync(DateTimeOffset utc)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetUTC",
            Utc = utc.ToUnixTimeSeconds()
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    public async Task<DeviceResult> ConfigTemperatureModeAsync(TemperatureMode mode)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetDisTempMode",
            Mode = (int)mode
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    public async Task<DeviceResult> ConfigHourModeAsync(HourMode mode)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetTime24Flag",
            Mode = (int)mode
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    //--------------------------------------------------------------------------------
    // Draw
    //--------------------------------------------------------------------------------

    public async Task<PictureIdResult> GetPictureIdAsync()
    {
        using var request = CreateRequest(new
        {
            Command = "Draw/GetHttpGifId"
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PictureIdResult>(json)!;
    }

    public async Task<DeviceResult> ResetPictureIdAsync()
    {
        using var request = CreateRequest(new
        {
            Command = "Draw/ResetHttpGifId"
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    public async Task<DeviceResult> SendImageAsync(
        int id,
        int width,
        string data,
        int num = 1,
        int offset = 0,
        int speed = 0)
    {
        using var request = CreateRequest(new
        {
            Command = "Draw/SendHttpGif",
            PicNum = num,
            PicWidth = width,
            PicOffset = offset,
            PicID = id,
            PicSpeed = speed,
            PicData = data
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    public async Task<DeviceResult> SendRemoteAsync(string fileId)
    {
        using var request = CreateRequest(new
        {
            Command = "Draw/SendRemote",
            FileId = fileId
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    public async Task<DeviceResult> SendTextAsync(
        int id,
        int x,
        int y,
        int width,
        int font,
        string color,
        string text,
        TextAlignment alignment = TextAlignment.Left,
        TextDirection direction = TextDirection.Left,
        int speed = 0)
    {
        using var request = CreateRequest(new
        {
            Command = "Draw/SendHttpText",
            TextId = id,
            x,
            y,
            dir = (int)direction,
            font,
            TextWidth = width,
            speed,
            TextString = text,
            color,
            align = (int)alignment
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    public async Task<DeviceResult> ClearTextAsync()
    {
        using var request = CreateRequest(new
        {
            Command = "Draw/ClearHttpText"
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    public async Task<DeviceResult> SendItemListAsync(IEnumerable<DrawItem> items)
    {
        using var request = CreateRequest(new
        {
            Command = "Draw/SendHttpItemList",
            ItemList = items.Select(static x => new
            {
                x.TextId,
                type = (int)x.Type,
                x = x.X,
                y = x.Y,
                dir = (int)x.Direction,
                font = x.Font,
                TextWidth = x.Width,
                TextHeight = x.Height,
                TextString = x.Text,
                speed = x.Speed,
                color = x.Color,
                update_time = x.UpdateInterval,
                align = (int)x.Alignment
            })
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }

    public async Task<DeviceResult> PlayGif(PlayFileType fileType, string fileName)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/PlayTFGif",
            FileType = (int)fileType,
            FileName = fileName
        });
        var response = await client.PostAsync(PostUrl, request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DeviceResult>(json)!;
    }
}
