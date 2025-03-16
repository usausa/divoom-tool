namespace Divoom.Client;

using System.Text.Json.Serialization;

public class Result
{
    [JsonPropertyName("error_code")]
    public int Code { get; set; }
}

public class IndexResult : Result
{
    [JsonPropertyName("SelectIndex")]
    public int Index { get; set; }
}

public class ClockResult : Result
{
    [JsonPropertyName("ClockId")]
    public int ClockId { get; set; }

    [JsonPropertyName("Brightness")]
    public int Brightness { get; set; }
}

public class TimeResult : Result
{
    [JsonPropertyName("UTCTime")]
    public long Utc { get; set; }

    [JsonPropertyName("LocalTime")]
    public string LocalTime { get; set; } = default!;
}

public class WeatherResult : Result
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

public class ConfigResult : Result
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

public class PictureIdResult : Result
{
    [JsonPropertyName("PicId")]
    public int PictureId { get; set; }
}
