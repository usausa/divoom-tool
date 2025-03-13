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
