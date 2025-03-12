namespace Divoom.Client;

using System.Text.Json.Serialization;

internal class DeviceRequest
{
    [JsonPropertyName("Command")]
    public string Command { get; set; } = default!;
}
