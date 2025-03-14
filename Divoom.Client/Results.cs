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
