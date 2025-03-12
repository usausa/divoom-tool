namespace Divoom.Client;

using System.Text.Json.Serialization;

public class Result
{
    [JsonPropertyName("error_code")]
    public int Code { get; set; }
}
