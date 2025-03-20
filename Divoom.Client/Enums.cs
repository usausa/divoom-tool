namespace Divoom.Client;

#pragma warning disable CA1008

public enum FontType
{
    None = 0,
    CanScroll = 1
}

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

public enum LightIndex
{
    All = 0,
    Edge = 1,
    Backlight = 2
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

public enum PlayFileType
{
    File = 0,
    Folder = 1,
    Net = 2
}

public enum DisplayType
{
    Second = 1,
    Minute = 2,
    Hour = 3,
    TimeAmPm = 4,
    HourMinute = 5,
    HourMinuteSecond = 6,
    Year = 7,
    Day = 8,
    Month = 9,
    MonthYear = 10,
    MonthDay = 11,
    DayMonthYear = 12,
    Week2 = 13,
    Week3 = 14,
    WeekFull = 15,
    MonthShort = 16,
    Temperature = 17,
    MaxTemperature = 18,
    MinTemperature = 19,
    Weather = 20,
    Noise = 21,
    Text = 22,
    NetText = 23
}
