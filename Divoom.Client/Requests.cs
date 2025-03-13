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

internal sealed class TimerRequest : DeviceRequest
{
    [JsonPropertyName("Minute")]
    public int Minute { get; set; }

    [JsonPropertyName("Second")]
    public int Second { get; set; }

    [JsonPropertyName("Status")]
    public int Status { get; set; }
}

internal sealed class StopwatchRequest : DeviceRequest
{
    [JsonPropertyName("Status")]
    public int Status { get; set; }
}

internal sealed class ScoreboardRequest : DeviceRequest
{
    [JsonPropertyName("BlueScore")]
    public int Blue { get; set; }

    [JsonPropertyName("RedScore")]
    public int Red { get; set; }
}

internal sealed class NoiseStatusRequest : DeviceRequest
{
    [JsonPropertyName("NoiseStatus")]
    public int Status { get; set; }
}

internal sealed class PlayBuzzerRequest : DeviceRequest
{
    [JsonPropertyName("ActiveTimeInCycle")]
    public int Active { get; set; }

    [JsonPropertyName("OffTimeInCycle")]
    public int Off { get; set; }

    [JsonPropertyName("PlayTotalTime")]
    public int Total { get; set; }
}

internal sealed class ScreenRequest : DeviceRequest
{
    [JsonPropertyName("OnOff")]
    public int OnOff { get; set; }
}
