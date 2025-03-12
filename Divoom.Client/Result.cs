namespace Divoom.Client;

using System.Text.Json.Serialization;

public class Result
{
    [JsonPropertyName("ReturnCode")]
    public int Code { get; set; }

    [JsonPropertyName("ReturnMessage")]
    public string Message { get; set; } = default!;
}
