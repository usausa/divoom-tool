namespace Divoom.Client;

using System.Text.Json.Serialization;

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

#pragma warning disable CA1812
internal sealed class DeviceListResult
{
    public DeviceInfo[] DeviceList { get; set; } = default!;
}
#pragma warning disable CA1812
