namespace Divoom.Client;

public enum Channel
{
    Clock,
    Cloud,
    Equalizer,
    Custom
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
