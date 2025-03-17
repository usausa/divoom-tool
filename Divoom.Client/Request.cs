namespace Divoom.Client;

using System.Text.Json.Serialization;

public sealed class DrawItem
{
    public int TextId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DisplayType Type { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public int Font { get; set; }

    public string Color { get; set; } = default!;

    public string Text { get; set; } = default!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TextAlignment Alignment { get; set; } = TextAlignment.Left;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TextDirection Direction { get; set; } = TextDirection.Left;

    public int Speed { get; set; }

    public int UpdateInterval { get; set; }
}
