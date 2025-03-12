namespace Divoom.Client;

using System.Text.Json.Serialization;

#pragma warning disable CA1819
public class DeviceListResult
{
    [JsonPropertyName("DeviceList")]
    public DeviceInfo[] Devices { get; set; } = default!;
}
#pragma warning restore CA1819
