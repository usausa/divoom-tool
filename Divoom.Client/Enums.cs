namespace Divoom.Client;

#pragma warning disable CA1008

public enum Channel
{
    Clock = 0,
    Cloud = 1,
    Equalizer = 2,
    Custom = 3
}

public enum CloudIndex
{
    Recommend = 0,
    Favourite = 1,
    Subscribe = 2,
    Album = 3
}

public enum StopwatchCommand
{
    Stop = 0,
    Start = 1,
    Reset = 2
}

public enum TemperatureMode
{
    Celsius = 0,
    Fahrenheit = 1
}

public enum HourMode
{
    Hour12 = 0,
    Hour24 = 1
}

public enum RotationAngle
{
    None = 0,
    Rotate90 = 1,
    Rotate180 = 2,
    Rotate270 = 3
}

public enum TextDirection
{
    Left = 0,
    Right = 1
}

public enum TextAlignment
{
    Left = 1,
    Middle = 2,
    Right = 3
}
