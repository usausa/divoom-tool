namespace Divoom.Client;

using System.Text.Json.Serialization;

public class CurrentChannelResult : Result
{
    [JsonPropertyName("SelectIndex")]
    public int Index { get; set; }
}
