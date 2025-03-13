namespace Divoom.Client;

using System.Text.Json.Serialization;

internal class DeviceRequest
{
    [JsonPropertyName("Command")]
    public string Command { get; set; } = default!;
}

internal sealed class IndexRequest : DeviceRequest
{
    [JsonPropertyName("SelectIndex")]
    public int Index { get; set; }
}

internal sealed class ScreenRequest : DeviceRequest
{
    [JsonPropertyName("OnOff")]
    public int OnOff { get; set; }
}

internal sealed class TimerRequest : DeviceRequest
{
    [JsonPropertyName("Minute")]
    public int Minute { get; set; }

    [JsonPropertyName("Second")]
    public int Second { get; set; }

    [JsonPropertyName("Status")]
    public int Status { get; set; }
}

internal sealed class StopWatchRequest : DeviceRequest
{
    [JsonPropertyName("Status")]
    public int Status { get; set; }
}
