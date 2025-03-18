namespace Divoom.Client;

using System.Text.Json.Serialization;

//--------------------------------------------------------------------------------
// Service
//--------------------------------------------------------------------------------

public class ServiceResult
{
    [JsonPropertyName("ReturnCode")]
    public int Code { get; set; }

    [JsonPropertyName("ReturnMessage")]
    public string Message { get; set; } = default!;
}

public sealed class DeviceInfo
{
    [JsonPropertyName("DeviceId")]
    public int Id { get; set; }

    [JsonPropertyName("Hardware")]
    public int Hardware { get; set; }

    [JsonPropertyName("DeviceName")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("DevicePrivateIP")]
    public string IpAddress { get; set; } = default!;

    [JsonPropertyName("DeviceMac")]
    public string MacAddress { get; set; } = default!;
}

#pragma warning disable CA1819
public sealed class DeviceListResult : ServiceResult
{
    [JsonPropertyName("DeviceList")]
    public DeviceInfo[] Devices { get; set; } = default!;
}
#pragma warning restore CA1819

public sealed class FontInfo
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("width")]
    public string Width { get; set; } = default!;

    [JsonPropertyName("high")]
    public string Height { get; set; } = default!;

    [JsonPropertyName("charset")]
    public string Chars { get; set; } = default!;

    [JsonPropertyName("type")]
    public FontType Type { get; set; }
}

#pragma warning disable CA1819
public sealed class FontListResult : ServiceResult
{
    [JsonPropertyName("FontList")]
    public FontInfo[] Fonts { get; set; } = default!;
}
#pragma warning restore CA1819

#pragma warning disable CA1819
public sealed class DialTypeResult : ServiceResult
{
    [JsonPropertyName("DialTypeList")]
    public string[] Types { get; set; } = default!;
}
#pragma warning restore CA1819

public sealed class DialInfo
{
    [JsonPropertyName("ClockId")]
    public int Id { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; } = default!;
}

#pragma warning disable CA1819
public sealed class DialListResult : ServiceResult
{
    [JsonPropertyName("TotalNum")]
    public int Total { get; set; }

    [JsonPropertyName("DialList")]
    public DialInfo[] Dials { get; set; } = default!;
}
#pragma warning restore CA1819

public sealed class ImageInfo
{
    [JsonPropertyName("FileName")]
    public string FileName { get; set; } = default!;

    [JsonPropertyName("FileId")]
    public string FileId { get; set; } = default!;
}

#pragma warning disable CA1819
public class ImageListResult : ServiceResult
{
    [JsonPropertyName("ImgList")]
    public ImageInfo[] Images { get; set; } = default!;
}
#pragma warning restore CA1819

//--------------------------------------------------------------------------------
// Device
//--------------------------------------------------------------------------------

public class DeviceResult
{
    [JsonPropertyName("error_code")]
    public int Code { get; set; }
}

public sealed class IndexResult : DeviceResult
{
    [JsonPropertyName("SelectIndex")]
    public int Index { get; set; }
}

public sealed class ClockResult : DeviceResult
{
    [JsonPropertyName("ClockId")]
    public int ClockId { get; set; }

    [JsonPropertyName("Brightness")]
    public int Brightness { get; set; }
}

public sealed class TimeResult : DeviceResult
{
    [JsonPropertyName("UTCTime")]
    public long Utc { get; set; }

    [JsonPropertyName("LocalTime")]
    public string LocalTime { get; set; } = default!;
}

public sealed class WeatherResult : DeviceResult
{
    [JsonPropertyName("Weather")]
    public string Weather { get; set; } = default!;

    [JsonPropertyName("CurTemp")]
    public double CurrentTemperature { get; set; }

    [JsonPropertyName("MinTemp")]
    public double MinTemperature { get; set; }

    [JsonPropertyName("MaxTemp")]
    public double MaxTemperature { get; set; }

    [JsonPropertyName("Pressure")]
    public double Pressure { get; set; }

    [JsonPropertyName("Humidity")]
    public double Humidity { get; set; }

    [JsonPropertyName("Visibility")]
    public double Visibility { get; set; }

    [JsonPropertyName("WindSpeed")]
    public double WindSpeed { get; set; }
}

public sealed class ConfigResult : DeviceResult
{
    [JsonPropertyName("Weather")]
    public string Weather { get; set; } = default!;

    [JsonPropertyName("Brightness")]
    public int Brightness { get; set; }

    [JsonPropertyName("RotationFlag")]
    public int Rotation { get; set; }

    [JsonPropertyName("ClockTime")]
    public int ClockTime { get; set; }

    [JsonPropertyName("GalleryTime")]
    public int GalleryTime { get; set; }

    [JsonPropertyName("SingleGalleyTime")]
    public int SingleGalleyTime { get; set; }

    [JsonPropertyName("PowerOnChannelId")]
    public int PowerOnChannelId { get; set; }

    [JsonPropertyName("GalleryShowTimeFlag")]
    public int GalleryShowTime { get; set; }

    [JsonPropertyName("CurClockId")]
    public int CurrentClockId { get; set; }

    [JsonPropertyName("Time24Flag")]
    public int Time24 { get; set; }

    [JsonPropertyName("TemperatureMode")]
    public int TemperatureMode { get; set; }

    [JsonPropertyName("GyrateAngle")]
    public int GyrateAngle { get; set; }

    [JsonPropertyName("MirrorFlag")]
    public int Mirror { get; set; }

    [JsonPropertyName("LightSwitch")]
    public int LightSwitch { get; set; }
}

public sealed class PictureIdResult : DeviceResult
{
    [JsonPropertyName("PicId")]
    public int PictureId { get; set; }
}
