using System.CommandLine;
using System.CommandLine.NamingConventionBinder;

using Divoom.Client;
using Divoom.Tools;

using SkiaSharp;

// ReSharper disable UseObjectOrCollectionInitializer

var rootCommand = new RootCommand("Divoom client");

//--------------------------------------------------------------------------------
// device
//--------------------------------------------------------------------------------
var deviceCommand = new Command("device", "Get lan device list");
deviceCommand.Handler = CommandHandler.Create(static async (IConsole console) =>
{
    var result = await DivoomClient.GetDeviceListAsync();
    result.EnsureSuccessStatus();

    foreach (var device in result.Devices)
    {
        console.WriteLine($"{device.Id} {device.MacAddress} {device.IpAddress} {device.Hardware} {device.Name}");
    }
});
rootCommand.Add(deviceCommand);

//--------------------------------------------------------------------------------
// current
//--------------------------------------------------------------------------------
var currentCommand = new Command("current", "Get current channel");
currentCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
currentCommand.Handler = CommandHandler.Create(static async (IConsole console, string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.GetChannelIndexAsync();
    result.EnsureSuccessStatus();

    console.WriteLine($"Index: {result.Index}");
});
rootCommand.Add(currentCommand);

//--------------------------------------------------------------------------------
// clock
//--------------------------------------------------------------------------------
var clockCommand = new Command("clock", "Clock channel");
clockCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
clockCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SetChannelIndexAsync(Channel.Clock);
    result.EnsureSuccessStatus();
});
rootCommand.Add(clockCommand);

var clockInfoCommand = new Command("info", "Show clock information");
clockInfoCommand.Handler = CommandHandler.Create(static async (IConsole console, string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.GetClockInfoAsync();
    result.EnsureSuccessStatus();

    console.WriteLine($"ClockId: {result.ClockId}");
    console.WriteLine($"Brightness: {result.Brightness}");
});
clockCommand.Add(clockInfoCommand);

var clockSelectCommand = new Command("select", "Select clock");
clockSelectCommand.AddOption(new Option<int>(["--id", "-i"], "Id") { IsRequired = true });
clockSelectCommand.Handler = CommandHandler.Create(static async (string host, int id) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SelectClockIdAsync(id);
    result.EnsureSuccessStatus();
});
clockCommand.Add(clockSelectCommand);

//--------------------------------------------------------------------------------
// cloud
//--------------------------------------------------------------------------------
var cloudCommand = new Command("cloud", "Cloud channel");
cloudCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
cloudCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SetChannelIndexAsync(Channel.Cloud);
    result.EnsureSuccessStatus();
});
rootCommand.Add(cloudCommand);

var cloudSelectCommand = new Command("select", "Select cloud page");
cloudSelectCommand.AddOption(new Option<int>(["--index", "-i"], "Page index") { IsRequired = true });
cloudSelectCommand.Handler = CommandHandler.Create(static async (string host, int index) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SelectCloudIndexAsync((CloudIndex)index);
    result.EnsureSuccessStatus();
});
cloudCommand.Add(cloudSelectCommand);

//--------------------------------------------------------------------------------
// equalizer
//--------------------------------------------------------------------------------
var equalizerCommand = new Command("equalizer", "Equalizer channel");
equalizerCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
equalizerCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SetChannelIndexAsync(Channel.Equalizer);
    result.EnsureSuccessStatus();
});
rootCommand.Add(equalizerCommand);

var equalizerSelectCommand = new Command("select", "Select equalizer");
equalizerSelectCommand.AddOption(new Option<int>(["--index", "-i"], "Equalizer index") { IsRequired = true });
equalizerSelectCommand.Handler = CommandHandler.Create(static async (string host, int index) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SelectEqualizerIdAsync(index);
    result.EnsureSuccessStatus();
});
equalizerCommand.Add(equalizerSelectCommand);

//--------------------------------------------------------------------------------
// custom
//--------------------------------------------------------------------------------
var customCommand = new Command("custom", "Custom channel");
customCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
customCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SetChannelIndexAsync(Channel.Custom);
    result.EnsureSuccessStatus();
});
rootCommand.Add(customCommand);

var customSelectCommand = new Command("select", "Select custom page");
customSelectCommand.AddOption(new Option<int>(["--index", "-i"], "Page index") { IsRequired = true });
customSelectCommand.Handler = CommandHandler.Create(static async (string host, int index) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SelectCustomPageAsync(index);
    result.EnsureSuccessStatus();
});
customCommand.Add(customSelectCommand);

//--------------------------------------------------------------------------------
// monitor
//--------------------------------------------------------------------------------
var monitorCommand = new Command("monitor", "Clock channel");
monitorCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
monitorCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SelectClockIdAsync(625);
    result.EnsureSuccessStatus();
});
rootCommand.Add(monitorCommand);

// TODO Update?

//--------------------------------------------------------------------------------
// timer
//--------------------------------------------------------------------------------
var timerCommand = new Command("timer", "Timer tool");
timerCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
rootCommand.Add(timerCommand);

var timerStartCommand = new Command("start", "Start timer");
timerStartCommand.AddOption(new Option<int>(["--second", "-s"], "Second") { IsRequired = true });
timerStartCommand.Handler = CommandHandler.Create(static async (string host, int second) =>
{
    using var client = new DivoomClient(host);
    var result = await client.TimerToolAsync(true, second);
    result.EnsureSuccessStatus();
});
timerCommand.Add(timerStartCommand);

var timerStopCommand = new Command("stop", "Stop timer");
timerStopCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.TimerToolAsync(false, 0);
    result.EnsureSuccessStatus();
});
timerCommand.Add(timerStopCommand);

//--------------------------------------------------------------------------------
// watch
//--------------------------------------------------------------------------------
var watchCommand = new Command("watch", "Stopwatch tool");
watchCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
rootCommand.Add(watchCommand);

var watchStartCommand = new Command("start", "Start stopwatch");
watchStartCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.StopwatchToolAsync(StopwatchCommand.Start);
    result.EnsureSuccessStatus();
});
watchCommand.Add(watchStartCommand);

var watchStopCommand = new Command("stop", "Stop stopwatch");
watchStopCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.StopwatchToolAsync(StopwatchCommand.Stop);
    result.EnsureSuccessStatus();
});
watchCommand.Add(watchStopCommand);

var watchResetCommand = new Command("reset", "Reset stopwatch");
watchResetCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.StopwatchToolAsync(StopwatchCommand.Reset);
    result.EnsureSuccessStatus();
});
watchCommand.Add(watchResetCommand);

//--------------------------------------------------------------------------------
// score
//--------------------------------------------------------------------------------
var scoreCommand = new Command("score", "Scoreboard tool");
scoreCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
scoreCommand.AddOption(new Option<int>(["--blue", "-b"], "Blue score") { IsRequired = true });
scoreCommand.AddOption(new Option<int>(["--red", "-r"], "Red score") { IsRequired = true });
scoreCommand.Handler = CommandHandler.Create(static async (string host, int blue, int red) =>
{
    using var client = new DivoomClient(host);
    var result = await client.ScoreboardToolAsync(blue, red);
    result.EnsureSuccessStatus();
});
rootCommand.Add(scoreCommand);

//--------------------------------------------------------------------------------
// noise
//--------------------------------------------------------------------------------
var noiseCommand = new Command("noise", "Noise status tool");
noiseCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
rootCommand.Add(noiseCommand);

var noiseStartCommand = new Command("start", "Start timer");
noiseStartCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.NoiseToolAsync(true);
    result.EnsureSuccessStatus();
});
noiseCommand.Add(noiseStartCommand);

var noiseStopCommand = new Command("stop", "Stop tomer");
noiseStopCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.NoiseToolAsync(false);
    result.EnsureSuccessStatus();
});
noiseCommand.Add(noiseStopCommand);

//--------------------------------------------------------------------------------
// time
//--------------------------------------------------------------------------------
var timeCommand = new Command("time", "Get device time");
timeCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
timeCommand.AddOption(new Option<string>(["--time", "-t"], "Local time"));
timeCommand.Handler = CommandHandler.Create(static async (IConsole console, string host, string time) =>
{
    using var client = new DivoomClient(host);
    if (String.IsNullOrEmpty(time))
    {
        var result = await client.GetDeviceTimeAsync();
        result.EnsureSuccessStatus();

        console.WriteLine($"UTC: {result.Utc}");
        console.WriteLine($"LocalTime: {result.LocalTime}");
    }
    else
    {
        var utc = time == "auto" ? DateTimeOffset.UtcNow : DateTimeOffset.Parse(time).ToUniversalTime();

        var result = await client.ConfigSystemTimeAsync(utc);
        result.EnsureSuccessStatus();
    }
});
rootCommand.Add(timeCommand);

//--------------------------------------------------------------------------------
// weather
//--------------------------------------------------------------------------------
var weatherCommand = new Command("weather", "Get device weather");
weatherCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
weatherCommand.Handler = CommandHandler.Create(static async (IConsole console, string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.GetWeatherInfoAsync();
    result.EnsureSuccessStatus();

    console.WriteLine($"Weather: {result.Weather}");
    console.WriteLine($"CurrentTemperature: {result.CurrentTemperature}");
    console.WriteLine($"MinTemperature: {result.MinTemperature}");
    console.WriteLine($"MaxTemperature: {result.MaxTemperature}");
    console.WriteLine($"Pressure: {result.Pressure}");
    console.WriteLine($"Humidity: {result.Humidity}");
    console.WriteLine($"Visibility: {result.Visibility}");
    console.WriteLine($"WindSpeed: {result.WindSpeed}");
});
rootCommand.Add(weatherCommand);

//--------------------------------------------------------------------------------
// buzzer
//--------------------------------------------------------------------------------
var buzzerCommand = new Command("buzzer", "Play buzzer");
buzzerCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
buzzerCommand.AddOption(new Option<int>(["--active", "-a"], () => 500, "Active time"));
buzzerCommand.AddOption(new Option<int>(["--off", "-f"], () => 500, "Off time"));
buzzerCommand.AddOption(new Option<int>(["--total", "-t"], () => 3000, "Total time"));
buzzerCommand.Handler = CommandHandler.Create(static async (string host, int active, int off, int total) =>
{
    using var client = new DivoomClient(host);
    var result = await client.PlayBuzzerAsync(active, off, total);
    result.EnsureSuccessStatus();
});
rootCommand.Add(buzzerCommand);

//--------------------------------------------------------------------------------
// screen
//--------------------------------------------------------------------------------
var screenCommand = new Command("screen", "Screen switch");
screenCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
rootCommand.Add(screenCommand);

var screenOnCommand = new Command("on", "Screen on");
screenOnCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SwitchScreenAsync(true);
    result.EnsureSuccessStatus();
});
screenCommand.Add(screenOnCommand);

var screenOffCommand = new Command("off", "Screen off");
screenOffCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SwitchScreenAsync(false);
    result.EnsureSuccessStatus();
});
screenCommand.Add(screenOffCommand);

//--------------------------------------------------------------------------------
// brightness
//--------------------------------------------------------------------------------
var brightnessCommand = new Command("brightness", "Set brightness");
brightnessCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
brightnessCommand.AddOption(new Option<int>(["--value", "-v"], "Brightness") { IsRequired = true });
brightnessCommand.Handler = CommandHandler.Create(static async (string host, int value) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SetBrightnessAsync(value);
    result.EnsureSuccessStatus();
});
rootCommand.Add(brightnessCommand);

//--------------------------------------------------------------------------------
// rotation
//--------------------------------------------------------------------------------
var rotationCommand = new Command("rotation", "Set rotation");
rotationCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
rotationCommand.AddOption(new Option<int>(["--rotation", "-r"], "Rotation") { IsRequired = true });
rotationCommand.Handler = CommandHandler.Create(static async (string host, int rotation) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SetScreenRotationAsync(
        rotation switch
        {
            90 => RotationAngle.Rotate90,
            180 => RotationAngle.Rotate180,
            270 => RotationAngle.Rotate270,
            _ => 0
        });
    result.EnsureSuccessStatus();
});
rootCommand.Add(rotationCommand);

//--------------------------------------------------------------------------------
// mirror
//--------------------------------------------------------------------------------
var mirrorCommand = new Command("mirror", "Set mirror mode");
mirrorCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
rootCommand.Add(mirrorCommand);

var mirrorOnCommand = new Command("on", "Mirror mode on");
mirrorOnCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SetMirrorModeAsync(true);
    result.EnsureSuccessStatus();
});
mirrorCommand.Add(mirrorOnCommand);

var mirrorOffCommand = new Command("off", "Mirror mode off");
mirrorOffCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SetMirrorModeAsync(false);
    result.EnsureSuccessStatus();
});
mirrorCommand.Add(mirrorOffCommand);

//--------------------------------------------------------------------------------
// highlight
//--------------------------------------------------------------------------------
var highlightCommand = new Command("highlight", "Set highlight mode");
highlightCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
rootCommand.Add(highlightCommand);

var highlightOnCommand = new Command("on", "Highlight mode on");
highlightOnCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SetHighlightModeAsync(true);
    result.EnsureSuccessStatus();
});
highlightCommand.Add(highlightOnCommand);

var highlightOffCommand = new Command("off", "Highlight mode off");
highlightOffCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SetHighlightModeAsync(false);
    result.EnsureSuccessStatus();
});
highlightCommand.Add(highlightOffCommand);

//--------------------------------------------------------------------------------
// white
//--------------------------------------------------------------------------------
var whiteCommand = new Command("white", "Set white");
whiteCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
whiteCommand.AddOption(new Option<int>(["--color", "-r"], "Red") { IsRequired = true });
whiteCommand.AddOption(new Option<int>(["--green", "-g"], "Green") { IsRequired = true });
whiteCommand.AddOption(new Option<int>(["--blue", "-b"], "Blue") { IsRequired = true });
whiteCommand.Handler = CommandHandler.Create(static async (string host, int red, int green, int blue) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SetWhiteBalanceAsync(red, blue, green);
    result.EnsureSuccessStatus();
});
rootCommand.Add(whiteCommand);

//--------------------------------------------------------------------------------
// config
//--------------------------------------------------------------------------------
var configCommand = new Command("config", "Get all config");
configCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
configCommand.Handler = CommandHandler.Create(static async (IConsole console, string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.GetAllConfigAsync();
    result.EnsureSuccessStatus();

    console.WriteLine($"Brightness: {result.Brightness}");
    var rotation = (RotationAngle)result.Rotation switch
    {
        RotationAngle.Rotate90 => 90,
        RotationAngle.Rotate180 => 180,
        RotationAngle.Rotate270 => 270,
        _ => 0
    };
    console.WriteLine($"Rotation: {rotation}");
    console.WriteLine($"ClockTime: {result.ClockTime}");
    console.WriteLine($"GalleryTime: {result.GalleryTime}");
    console.WriteLine($"SingleGalleyTime: {result.SingleGalleyTime}");
    console.WriteLine($"PowerOnChannelId: {result.PowerOnChannelId}");
    console.WriteLine($"GalleryShowTime: {result.GalleryShowTime}");
    console.WriteLine($"CurrentClockId: {result.CurrentClockId}");
    console.WriteLine($"HourMode: {(HourMode)result.Time24}");
    console.WriteLine($"TemperatureMode: {(TemperatureMode)result.TemperatureMode}");
    console.WriteLine($"GyrateAngle: {result.GyrateAngle}");
    var mirror = result.Mirror == 1 ? "enable" : "disable";
    console.WriteLine($"Mirror: {mirror}");
    var lightSwitch = result.LightSwitch == 1 ? "on" : "off";
    console.WriteLine($"LightSwitch: {lightSwitch}");
});
rootCommand.Add(configCommand);

//--------------------------------------------------------------------------------
// area
//--------------------------------------------------------------------------------
var areaCommand = new Command("area", "Set area");
areaCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
areaCommand.AddOption(new Option<double>(["--lon", "-n"], "Longitude") { IsRequired = true });
areaCommand.AddOption(new Option<double>(["--lat", "-t"], "Latitude") { IsRequired = true });
areaCommand.Handler = CommandHandler.Create(static async (string host, double lon, double lat) =>
{
    using var client = new DivoomClient(host);
    var result = await client.ConfigLogAndLatAsync(lon, lat);
    result.EnsureSuccessStatus();
});
rootCommand.Add(areaCommand);

//--------------------------------------------------------------------------------
// timezone
//--------------------------------------------------------------------------------
var timezoneCommand = new Command("timezone", "Set timezone");
timezoneCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
timezoneCommand.AddOption(new Option<string>(["--value", "-v"], "Timezone") { IsRequired = true });
timezoneCommand.Handler = CommandHandler.Create(static async (string host, string value) =>
{
    using var client = new DivoomClient(host);
    var result = await client.ConfigTimeZoneAsync(value);
    result.EnsureSuccessStatus();
});
rootCommand.Add(timezoneCommand);

//--------------------------------------------------------------------------------
// temperature
//--------------------------------------------------------------------------------
var temperatureCommand = new Command("temperature", "Set temperature mode");
temperatureCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
temperatureCommand.AddOption(new Option<string>(["--mode", "-m"], "Mode") { IsRequired = true }.FromAmong("c", "celsius", "f", "fahrenheit"));
temperatureCommand.Handler = CommandHandler.Create(static async (string host, string value) =>
{
    using var client = new DivoomClient(host);
    var result = await client.ConfigTemperatureModeAsync(
        value switch
        {
            "f" or "fahrenheit" => TemperatureMode.Fahrenheit,
            _ => TemperatureMode.Celsius
        });
    result.EnsureSuccessStatus();
});
rootCommand.Add(temperatureCommand);

//--------------------------------------------------------------------------------
// hour
//--------------------------------------------------------------------------------
var hourCommand = new Command("hour", "Set hour mode");
hourCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
hourCommand.AddOption(new Option<string>(["--mode", "-m"], "Mode") { IsRequired = true }.FromAmong("12", "24"));
hourCommand.Handler = CommandHandler.Create(static async (string host, string value) =>
{
    using var client = new DivoomClient(host);
    var result = await client.ConfigHourModeAsync(
        value switch
        {
            "24" => HourMode.Hour24,
            _ => HourMode.Hour12
        });
    result.EnsureSuccessStatus();
});
rootCommand.Add(hourCommand);

//--------------------------------------------------------------------------------
// text
//--------------------------------------------------------------------------------
var textCommand = new Command("text", "Text tool");
textCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
rootCommand.Add(textCommand);

var textDrawCommand = new Command("draw", "Draw text");
textDrawCommand.AddOption(new Option<int>(["--id", "-i"], "Id") { IsRequired = true });
textDrawCommand.AddOption(new Option<int>(["-x"], "Start x") { IsRequired = true });
textDrawCommand.AddOption(new Option<int>(["-y"], "Start y") { IsRequired = true });
textDrawCommand.AddOption(new Option<int>(["--width", "-w"], "Text area width") { IsRequired = true });
textDrawCommand.AddOption(new Option<int>(["--font", "-f"], "Font id") { IsRequired = true });
textDrawCommand.AddOption(new Option<string>(["--color", "-c"], "Font color") { IsRequired = true });
textDrawCommand.AddOption(new Option<string>(["--text", "-t"], "Text string") { IsRequired = true });
textDrawCommand.AddOption(new Option<string>(["--ali", "-a"], () => "l", "Text alignment") { IsRequired = true }.FromAmong("l", "left", "m", "middle", "r", "right"));
textDrawCommand.AddOption(new Option<string>(["--dir", "-d"], () => "l", "Scroll direction") { IsRequired = true }.FromAmong("l", "left", "r", "right"));
textDrawCommand.AddOption(new Option<int>(["--speed", "-s"], () => 0, "Font id"));
textDrawCommand.Handler = CommandHandler.Create(static async (string host, int id, int x, int y, int width, int font, string color, string text, string alignment, string direction, int speed) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SendTextAsync(
        id,
        x,
        y,
        width,
        font,
        color.StartsWith('#') ? color : "#" + color,
        text,
        // TODO
        alignment == "m" ? TextAlignment.Middle : alignment == "r" ? TextAlignment.Right : TextAlignment.Left,
        direction == "r" ? TextDirection.Right : TextDirection.Left,
        speed);
    result.EnsureSuccessStatus();
});
textCommand.Add(textDrawCommand);

var textClearCommand = new Command("clear", "Clear text");
textClearCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.ClearTextAsync();
    result.EnsureSuccessStatus();
});
textCommand.Add(textClearCommand);

//--------------------------------------------------------------------------------
// image
//--------------------------------------------------------------------------------
var imageCommand = new Command("image", "Image tool");
imageCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
rootCommand.Add(imageCommand);

var imageResetCommand = new Command("reset", "Reset image id");
imageResetCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.ResetPictureIdAsync();
    result.EnsureSuccessStatus();
});
imageCommand.Add(imageResetCommand);

var imageIdCommand = new Command("id", "Get image id");
imageIdCommand.Handler = CommandHandler.Create(static async (IConsole console, string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.GetPictureIdAsync();
    result.EnsureSuccessStatus();

    console.WriteLine($"Id: {result.PictureId}");
});
imageCommand.Add(imageIdCommand);

var imageDrawCommand = new Command("draw", "Draw image");
imageDrawCommand.AddOption(new Option<int?>(["--id", "-i"], "Id"));
imageDrawCommand.AddOption(new Option<string>(["--file", "-f"], "File") { IsRequired = true });
imageDrawCommand.Handler = CommandHandler.Create(static async (string host, int? id, string file) =>
{
    using var client = new DivoomClient(host);

    if (!id.HasValue)
    {
        var idResult = await client.GetPictureIdAsync();
        idResult.EnsureSuccessStatus();
        id = idResult.PictureId;
    }

    await using var stream = File.OpenRead(file);
    using var bitmap = SKBitmap.Decode(stream);
    var buffer = new byte[bitmap.Width * bitmap.Height * 3];
    for (var y = 0; y < bitmap.Height; y++)
    {
        for (var x = 0; x < bitmap.Width; x++)
        {
            var span = buffer.AsSpan(((y * bitmap.Height) + x) * 3);
            var c = bitmap.GetPixel(x, y);
            span[0] = c.Red;
            span[1] = c.Green;
            span[2] = c.Blue;
        }
    }
    var data = Convert.ToBase64String(buffer);

    var result = await client.SendImageAsync(id.Value, bitmap.Width, data);
    result.EnsureSuccessStatus();
});
imageCommand.Add(imageDrawCommand);

var imageFillCommand = new Command("fill", "Fill image");
imageFillCommand.AddOption(new Option<int?>(["--id", "-i"], "Id"));
imageFillCommand.AddOption(new Option<int>(["--size", "-s"], () => 64, "Size"));
imageFillCommand.AddOption(new Option<string>(["--color", "-c"], () => "#000000", "Color"));
imageFillCommand.Handler = CommandHandler.Create(static async (string host, int? id, int size, string color) =>
{
    var c = SKColor.Parse(color);

    using var client = new DivoomClient(host);

    if (!id.HasValue)
    {
        var idResult = await client.GetPictureIdAsync();
        idResult.EnsureSuccessStatus();
        id = idResult.PictureId;
    }

    var buffer = new byte[size * size * 3];
    for (var offset = 0; offset < buffer.Length; offset += 3)
    {
        var span = buffer.AsSpan(offset);
        span[0] = c.Red;
        span[1] = c.Green;
        span[2] = c.Blue;
    }
    var data = Convert.ToBase64String(buffer);

    var result = await client.SendImageAsync(id.Value, size, data);
    result.EnsureSuccessStatus();
});
imageCommand.Add(imageFillCommand);

//--------------------------------------------------------------------------------
// remote
//--------------------------------------------------------------------------------
var remoteCommand = new Command("remote", "Remote image");
rootCommand.Add(remoteCommand);

var remoteListCommand = new Command("list", "List upload images");
remoteListCommand.AddOption(new Option<int>(["--device", "-d"], "Device id") { IsRequired = true });
remoteListCommand.AddOption(new Option<string>(["--mac", "-m"], "Device mac") { IsRequired = true });
remoteListCommand.AddOption(new Option<int>(["--page", "-p"], () => 1, "Page"));
remoteListCommand.Handler = CommandHandler.Create(static async (IConsole console, int device, string mac, int page) =>
{
    var result = await DivoomClient.GetUploadImageListAsync(device, mac, page);
    result.EnsureSuccessStatus();

    foreach (var image in result.Images)
    {
        console.WriteLine($"{image.FileName} {image.FileId}");
    }
});
remoteCommand.Add(remoteListCommand);

var remoteLikeCommand = new Command("like", "List like images");
remoteLikeCommand.AddOption(new Option<int>(["--device", "-d"], "Device id") { IsRequired = true });
remoteLikeCommand.AddOption(new Option<string>(["--mac", "-m"], "Device mac") { IsRequired = true });
remoteLikeCommand.AddOption(new Option<int>(["--page", "-p"], () => 1, "Page"));
remoteLikeCommand.Handler = CommandHandler.Create(static async (IConsole console, int device, string mac, int page) =>
{
    var result = await DivoomClient.GetLikeImageListAsync(device, mac, page);
    result.EnsureSuccessStatus();

    foreach (var image in result.Images)
    {
        console.WriteLine($"{image.FileName} {image.FileId}");
    }
});
remoteCommand.Add(remoteLikeCommand);

var remoteDrawCommand = new Command("draw", "Draw remote image");
remoteDrawCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
remoteDrawCommand.AddOption(new Option<string>(["--id", "-i"], "File id") { IsRequired = true });
remoteDrawCommand.Handler = CommandHandler.Create(static async (string host, string id) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SendRemoteAsync(id);
    result.EnsureSuccessStatus();
});
remoteCommand.Add(remoteDrawCommand);

//--------------------------------------------------------------------------------
// display
//--------------------------------------------------------------------------------

// TODO display

//--------------------------------------------------------------------------------
// gif
//--------------------------------------------------------------------------------
var gifCommand = new Command("gif", "Play gif");
gifCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
gifCommand.AddOption(new Option<string>(["--type", "-t"], "File type") { IsRequired = true }.FromAmong("n", "net", "d", "directory", "f", "file"));
gifCommand.AddOption(new Option<string>(["--name", "-n"], "File name") { IsRequired = true });
gifCommand.Handler = CommandHandler.Create(static async (string host, string type, string name) =>
{
    using var client = new DivoomClient(host);
    var result = await client.PlayGif(
        type switch
        {
            "d" or "directory" => PlayFileType.Folder,
            "f" or "file" => PlayFileType.File,
            _ => PlayFileType.Net
        },
        name);
    result.EnsureSuccessStatus();
});
rootCommand.Add(gifCommand);

//--------------------------------------------------------------------------------
// Run
//--------------------------------------------------------------------------------
var result = await rootCommand.InvokeAsync(args).ConfigureAwait(false);
#if DEBUG
Console.ReadLine();
#endif
return result;
