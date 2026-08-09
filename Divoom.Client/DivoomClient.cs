namespace Divoom.Client;

using System;
using System.Text.Json;

#pragma warning disable CA2234
public sealed class DivoomClient : IDisposable
{
    private const string PostUrl = "post";

    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);

    private readonly HttpClient client = new() { Timeout = DefaultTimeout };

    public TimeSpan Timeout
    {
        get => client.Timeout;
        set => client.Timeout = value;
    }

    public DivoomClient(string host)
    {
        var builder = new UriBuilder("http", host, 80);
        client.BaseAddress = builder.Uri;
    }

    public void Dispose()
    {
        client.Dispose();
    }

    private static StringContent CreateRequest(object request) =>
        new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

    private static async Task<T> ReadResultAsync<T>(HttpResponseMessage response, CancellationToken cancel)
    {
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancel).ConfigureAwait(false);
        try
        {
            return JsonSerializer.Deserialize<T>(json) ??
                   throw new DivoomClientException($"Response is empty. type=[{typeof(T).Name}], response=[{Abbreviate(json)}]");
        }
        catch (JsonException e)
        {
            throw new DivoomClientException($"Response is not valid json. type=[{typeof(T).Name}], response=[{Abbreviate(json)}]", e);
        }
    }

    private static string Abbreviate(string value) =>
        value.Length <= 256 ? value : String.Concat(value.AsSpan(0, 256), "...");

    //--------------------------------------------------------------------------------
    // Service
    //--------------------------------------------------------------------------------

    private static HttpClient CreateServiceClient() => new() { BaseAddress = new("http://app.divoom-gz.com"), Timeout = DefaultTimeout };

    public static async Task<DeviceListResult> GetDeviceListAsync(CancellationToken cancel = default)
    {
        using var client = CreateServiceClient();
        var response = await client.GetAsync("Device/ReturnSameLANDevice", cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceListResult>(response, cancel).ConfigureAwait(false);
    }

    public static async Task<FontListResult> GetFontListAsync(CancellationToken cancel = default)
    {
        using var client = CreateServiceClient();
        var response = await client.GetAsync("Device/GetTimeDialFontList", cancel).ConfigureAwait(false);
        return await ReadResultAsync<FontListResult>(response, cancel).ConfigureAwait(false);
    }

    public static async Task<ClockTypeResult> GetClockTypeAsync(CancellationToken cancel = default)
    {
        using var client = CreateServiceClient();
        var response = await client.GetAsync("Channel/GetDialType", cancel).ConfigureAwait(false);
        return await ReadResultAsync<ClockTypeResult>(response, cancel).ConfigureAwait(false);
    }

    public static async Task<ClockListResult> GetClockListAsync(string dialType, string? deviceType, int page, CancellationToken cancel = default)
    {
        using var client = CreateServiceClient();
        using var request = CreateRequest(new
        {
            DialType = dialType,
            DeviceType = deviceType,
            Page = page
        });
        var response = await client.PostAsync("Channel/GetDialList", request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<ClockListResult>(response, cancel).ConfigureAwait(false);
    }

    public static async Task<Lcd5ClockListResult> GetLcd5ClockListAsync(int page, CancellationToken cancel = default)
    {
        using var client = CreateServiceClient();
        using var request = CreateRequest(new
        {
            Page = page
        });
        var response = await client.PostAsync("Channel/Get5LcdClockListForCommon", request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<Lcd5ClockListResult>(response, cancel).ConfigureAwait(false);
    }

    public static async Task<Lcd5ClockInfoResult> GetLcd5ClockInfoAsync(int deviceId, string deviceType, CancellationToken cancel = default)
    {
        using var client = CreateServiceClient();
        using var request = CreateRequest(new
        {
            DeviceId = deviceId,
            DeviceType = deviceType
        });
        var response = await client.PostAsync("Channel/Get5LcdInfoV2", request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<Lcd5ClockInfoResult>(response, cancel).ConfigureAwait(false);
    }

    public static async Task<ImageListResult> GetUploadImageListAsync(int deviceId, string deviceMac, int page = 1, CancellationToken cancel = default)
    {
        using var client = CreateServiceClient();
        using var request = CreateRequest(new
        {
            DeviceId = deviceId,
            DeviceMac = deviceMac,
            Page = page
        });
        var response = await client.PostAsync("Device/GetImgUploadList", request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<ImageListResult>(response, cancel).ConfigureAwait(false);
    }

    public static async Task<ImageListResult> GetLikeImageListAsync(int deviceId, string deviceMac, int page = 1, CancellationToken cancel = default)
    {
        using var client = CreateServiceClient();
        using var request = CreateRequest(new
        {
            DeviceId = deviceId,
            DeviceMac = deviceMac,
            Page = page
        });
        var response = await client.PostAsync("Device/GetImgLikeList", request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<ImageListResult>(response, cancel).ConfigureAwait(false);
    }

    //--------------------------------------------------------------------------------
    // Reboot
    //--------------------------------------------------------------------------------

    public async Task<IndexResult> RebootAsync(CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SysReboot"
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<IndexResult>(response, cancel).ConfigureAwait(false);
    }

    //--------------------------------------------------------------------------------
    // Channel
    //--------------------------------------------------------------------------------

    public async Task<IndexResult> GetChannelIndexAsync(CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/GetIndex"
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<IndexResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> SetChannelIndexAsync(IndexType index, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/SetIndex",
            SelectIndex = (int)index
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    //--------------------------------------------------------------------------------
    // Lcd5
    //--------------------------------------------------------------------------------

    public async Task<IndexResult> SetLcd5ChannelTypeAsync(Lcd5ChannelType channelType, int? lcdId, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/Set5LcdChannelType",
            ChannelType = (int)channelType,
            LcdIndependence = lcdId
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<IndexResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> SelectLcd5WholeClockIdIdAsync(int clockId, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/Set5LcdWholeClockId",
            ClockId = clockId
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    //--------------------------------------------------------------------------------
    // Clock
    //--------------------------------------------------------------------------------

    public async Task<ClockResult> GetClockInfoAsync(CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/GetClockInfo"
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<ClockResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> SelectClockIdAsync(int clockId, int? lcdId = null, int? lcdIndex = null, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/SetClockSelectId",
            ClockId = clockId,
            LcdIndependence = lcdId,
            LcdIndex = lcdIndex
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    //--------------------------------------------------------------------------------
    // Cloud
    //--------------------------------------------------------------------------------

    public async Task<DeviceResult> SelectCloudIndexAsync(CloudIndex page, int? lcdId = null, int? lcdIndex = null, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/CloudIndex",
            CustomPageIndex = (int)page,
            LcdIndependence = lcdId,
            LcdIndex = lcdIndex
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    //--------------------------------------------------------------------------------
    // Equalizer
    //--------------------------------------------------------------------------------

    public async Task<DeviceResult> SelectEqualizerIdAsync(int position, int? lcdId = null, int? lcdIndex = null, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/SetEqPosition",
            EqPosition = position,
            LcdIndependence = lcdId,
            LcdIndex = lcdIndex
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    //--------------------------------------------------------------------------------
    // Custom
    //--------------------------------------------------------------------------------

    public async Task<DeviceResult> SelectCustomPageAsync(int page, int? lcdId = null, int? lcdIndex = null, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/SetCustomPageIndex",
            CustomPageIndex = page,
            LcdIndependence = lcdId,
            LcdIndex = lcdIndex
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    //--------------------------------------------------------------------------------
    // Monitor
    //--------------------------------------------------------------------------------

    public async Task<DeviceResult> UpdatePcMonitorAsync(IEnumerable<MonitorParameter> parameters, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/UpdatePCParaInfo",
            ScreenList = parameters.Select(static x => new
            {
                LcdId = x.Lcd,
                DispData = new[] { x.CpuUsed, x.GpuUsed, x.CpuTemperature, x.GpuTemperature, x.MemoryUsed, x.DiskTemperature }
            })
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    //--------------------------------------------------------------------------------
    // Tool
    //--------------------------------------------------------------------------------

    public async Task<DeviceResult> TimerToolAsync(bool enable, int second, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Tools/SetTimer",
            Minute = second / 60,
            Second = second % 60,
            Status = enable ? 1 : 0
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> StopwatchToolAsync(StopwatchCommand command, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Tools/SetStopWatch",
            Status = (int)command
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> ScoreboardToolAsync(int blue, int red, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Tools/SetScoreBoard",
            BlueScore = blue,
            RedScore = red
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> NoiseToolAsync(bool enable, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Tools/SetNoiseStatus",
            NoiseStatus = enable ? 1 : 0
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    //--------------------------------------------------------------------------------
    // Setting
    //--------------------------------------------------------------------------------

    public async Task<TimeResult> GetDeviceTimeAsync(CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/GetDeviceTime"
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<TimeResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<WeatherResult> GetWeatherInfoAsync(CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/GetWeatherInfo"
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<WeatherResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> PlayBuzzerAsync(int activeTime, int offTime, int totalTime, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/PlayBuzzer",
            ActiveTimeInCycle = activeTime,
            OffTimeInCycle = offTime,
            PlayTotalTime = totalTime
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> SwitchScreenAsync(bool on, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/OnOffScreen",
            OnOff = on ? 1 : 0
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> SetBrightnessAsync(int brightness, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/SetBrightness",
            Brightness = brightness
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> SetScreenRotationAsync(RotationAngle rotation, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetScreenRotationAngle",
            Mode = (int)rotation
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> SetMirrorModeAsync(bool on, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetMirrorMode",
            Mode = on ? 1 : 0
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> SetHighlightModeAsync(bool on, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetHighLightMode",
            Mode = on ? 1 : 0
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> SetWhiteBalanceAsync(int red, int green, int blue, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetWhiteBalance",
            RValue = red,
            GValue = green,
            BValue = blue
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> SetRgbInformationAsync(int brightness, string color, bool light, bool key, bool cycle, LightIndex index, IEnumerable<int> effect, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/SetRGBInfo",
            Brightness = brightness,
            Color = color,
            OnOff = light ? 1 : 0,
            KeyOnOff = key ? 1 : 0,
            ColorCycle = cycle ? 1 : 0,
            SelectLightIndex = (int)index,
            LightList = effect.Select(static x => new { SelectEffect = x })
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<ConfigResult> GetAllConfigAsync(CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Channel/GetAllConf"
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<ConfigResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> ConfigLogAndLatAsync(double lon, double lat, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Sys/LogAndLat",
            Longitude = lon,
            Latitude = lat
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> ConfigTimeZoneAsync(string timezone, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Sys/TimeZone",
            TimeZoneValue = timezone
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> ConfigSystemTimeAsync(DateTimeOffset utc, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetUTC",
            Utc = utc.ToUnixTimeSeconds()
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> ConfigTemperatureModeAsync(TemperatureMode mode, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetDisTempMode",
            Mode = (int)mode
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> ConfigHourModeAsync(HourMode mode, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/SetTime24Flag",
            Mode = (int)mode
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    //--------------------------------------------------------------------------------
    // Draw
    //--------------------------------------------------------------------------------

    public async Task<PictureIdResult> GetPictureIdAsync(CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Draw/GetHttpGifId"
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<PictureIdResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> ResetPictureIdAsync(CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Draw/ResetHttpGifId"
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> SendImageAsync(
        int id,
        int width,
        string data,
        int num = 1,
        int offset = 0,
        int speed = 0,
        CancellationToken cancel = default)
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
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> SendRemoteAsync(string fileId, int[]? array, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Draw/SendRemote",
            FileId = fileId,
            LcdArray = array
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
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
        int speed = 0,
        CancellationToken cancel = default)
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
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> ClearTextAsync(CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Draw/ClearHttpText"
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> SendItemListAsync(IEnumerable<DrawItem> items, CancellationToken cancel = default)
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
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> PlayGifAsync(PlayFileType fileType, string fileName, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/PlayTFGif",
            FileType = (int)fileType,
            FileName = fileName
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> PlayGifArrayAsync(int[] array, string[] urls, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/PlayGif",
            LcdArray = array,
            FileName = urls
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }

    public async Task<DeviceResult> PlayGifAllLcdAsync(string[] lcd1, string[] lcd2, string[] lcd3, string[] lcd4, string[] lcd5, CancellationToken cancel = default)
    {
        using var request = CreateRequest(new
        {
            Command = "Device/PlayGifLCDs",
            LCD0GifFile = lcd1,
            LCD1GifFile = lcd2,
            LCD2GifFile = lcd3,
            LCD3GifFile = lcd4,
            LCD4GifFile = lcd5
        });
        var response = await client.PostAsync(PostUrl, request, cancel).ConfigureAwait(false);
        return await ReadResultAsync<DeviceResult>(response, cancel).ConfigureAwait(false);
    }
}
