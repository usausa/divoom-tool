using System.CommandLine;
using System.CommandLine.NamingConventionBinder;

using Divoom.Client;
using Divoom.Tools;

// ReSharper disable UseObjectOrCollectionInitializer

var rootCommand = new RootCommand("Divoom client");

//--------------------------------------------------------------------------------
// device
//--------------------------------------------------------------------------------
var deviceCommand = new Command("device", "Get lan device list");
deviceCommand.Handler = CommandHandler.Create(static async (IConsole console) =>
{
    using var client = new ServiceClient();
    var result = await client.GetDeviceListAsync();
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
    using var client = new DeviceClient(host);
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
    using var client = new DeviceClient(host);
    var result = await client.SetChannelIndexAsync(Channel.Clock);
    result.EnsureSuccessStatus();
});
rootCommand.Add(clockCommand);

//--------------------------------------------------------------------------------
// equalizer
//--------------------------------------------------------------------------------
var equalizerCommand = new Command("equalizer", "Equalizer channel");
equalizerCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
equalizerCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DeviceClient(host);
    var result = await client.SetChannelIndexAsync(Channel.Equalizer);
    result.EnsureSuccessStatus();
});
rootCommand.Add(equalizerCommand);

// TODO

//--------------------------------------------------------------------------------
// select
//--------------------------------------------------------------------------------
//var selectCommand = new Command("select", "Select channel");
//selectCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
//selectCommand.AddGlobalOption(new Option<int>(["--index", "-i"], "Index") { IsRequired = true });
//selectCommand.Handler = CommandHandler.Create(static async (IConsole console, string host, int index) =>
//{
//    // TODO ?
//    await Task.Delay(0);
//});
//rootCommand.Add(selectCommand);

//--------------------------------------------------------------------------------
// timer
//--------------------------------------------------------------------------------
var timerCommand = new Command("timer", "Timer tool");
timerCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
rootCommand.Add(timerCommand);

var timerStartCommand = new Command("start", "Timer start");
timerStartCommand.AddGlobalOption(new Option<int>(["--second", "-s"], "Second") { IsRequired = true });
timerStartCommand.Handler = CommandHandler.Create(static async (string host, int second) =>
{
    using var client = new DeviceClient(host);
    var result = await client.SetTimerAsync(true, second);
    result.EnsureSuccessStatus();
});
timerCommand.Add(timerStartCommand);

var timerStopCommand = new Command("stop", "Timer stop");
timerStopCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DeviceClient(host);
    var result = await client.SetTimerAsync(false, 0);
    result.EnsureSuccessStatus();
});
timerCommand.Add(timerStopCommand);

//--------------------------------------------------------------------------------
// watch
//--------------------------------------------------------------------------------
var watchCommand = new Command("watch", "Stopwatch tool");
watchCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
rootCommand.Add(watchCommand);

var watchStartCommand = new Command("start", "Stopwatch start");
watchStartCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DeviceClient(host);
    var result = await client.SetStopwatchAsync(StopwatchCommand.Start);
    result.EnsureSuccessStatus();
});
watchCommand.Add(watchStartCommand);

var watchStopCommand = new Command("stop", "Stopwatch stop");
watchStopCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DeviceClient(host);
    var result = await client.SetStopwatchAsync(StopwatchCommand.Stop);
    result.EnsureSuccessStatus();
});
watchCommand.Add(watchStopCommand);

var watchResetCommand = new Command("reset", "Stopwatch reset");
watchResetCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DeviceClient(host);
    var result = await client.SetStopwatchAsync(StopwatchCommand.Reset);
    result.EnsureSuccessStatus();
});
watchCommand.Add(watchResetCommand);

//--------------------------------------------------------------------------------
// score
//--------------------------------------------------------------------------------
var scoreCommand = new Command("score", "Scoreboard tool");
scoreCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
scoreCommand.AddGlobalOption(new Option<int>(["--blue", "-b"], "Blue score") { IsRequired = true });
scoreCommand.AddGlobalOption(new Option<int>(["--red", "-r"], "Red score") { IsRequired = true });
scoreCommand.Handler = CommandHandler.Create(static async (string host, int blue, int red) =>
{
    using var client = new DeviceClient(host);
    var result = await client.SetScoreboardAsync(blue, red);
    result.EnsureSuccessStatus();
});
rootCommand.Add(scoreCommand);

//--------------------------------------------------------------------------------
// noise
//--------------------------------------------------------------------------------
var noiseCommand = new Command("noise", "Noise status tool");
noiseCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
rootCommand.Add(noiseCommand);

var noiseStartCommand = new Command("start", "Timer start");
noiseStartCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DeviceClient(host);
    var result = await client.SetNoiseStatusAsync(true);
    result.EnsureSuccessStatus();
});
noiseCommand.Add(noiseStartCommand);

var noiseStopCommand = new Command("stop", "Timer stop");
noiseStopCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DeviceClient(host);
    var result = await client.SetNoiseStatusAsync(false);
    result.EnsureSuccessStatus();
});
noiseCommand.Add(noiseStopCommand);

//--------------------------------------------------------------------------------
// time
//--------------------------------------------------------------------------------
var timeCommand = new Command("time", "Get device time");
timeCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
timeCommand.Handler = CommandHandler.Create(static async (IConsole console, string host) =>
{
    using var client = new DeviceClient(host);
    var result = await client.GetDeviceTimeAsync();
    result.EnsureSuccessStatus();

    console.WriteLine($"UTC: {result.Utc}");
    console.WriteLine($"LocalTime: {result.LocalTime}");
});
rootCommand.Add(timeCommand);

//--------------------------------------------------------------------------------
// weather
//--------------------------------------------------------------------------------
var weatherCommand = new Command("weather", "Get device weather");
weatherCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
weatherCommand.Handler = CommandHandler.Create(static async (IConsole console, string host) =>
{
    using var client = new DeviceClient(host);
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
buzzerCommand.AddGlobalOption(new Option<int>(["--active", "-a"], () => 500, "Active time"));
buzzerCommand.AddGlobalOption(new Option<int>(["--off", "-f"], () => 500, "Off time"));
buzzerCommand.AddGlobalOption(new Option<int>(["--total", "-t"], () => 3000, "Total time"));
buzzerCommand.Handler = CommandHandler.Create(static async (string host, int active, int off, int total) =>
{
    using var client = new DeviceClient(host);
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
    using var client = new DeviceClient(host);
    var result = await client.SwitchScreenAsync(true);
    result.EnsureSuccessStatus();
});
screenCommand.Add(screenOnCommand);

var screenOffCommand = new Command("off", "Screen off");
screenOffCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DeviceClient(host);
    var result = await client.SwitchScreenAsync(false);
    result.EnsureSuccessStatus();
});
screenCommand.Add(screenOffCommand);

//--------------------------------------------------------------------------------
// brightness
//--------------------------------------------------------------------------------
var brightnessCommand = new Command("brightness", "Set brightness");
brightnessCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
brightnessCommand.AddGlobalOption(new Option<int>(["--brightness", "-b"], "Brightness") { IsRequired = true });
brightnessCommand.Handler = CommandHandler.Create(static async (string host, int brightness) =>
{
    using var client = new DeviceClient(host);
    var result = await client.SetBrightnessAsync(brightness);
    result.EnsureSuccessStatus();
});
rootCommand.Add(brightnessCommand);

//--------------------------------------------------------------------------------
// rotation
//--------------------------------------------------------------------------------
var rotationCommand = new Command("rotation", "Set rotation");
rotationCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
rotationCommand.AddGlobalOption(new Option<int>(["--rotation", "-r"], "Rotation") { IsRequired = true });
rotationCommand.Handler = CommandHandler.Create(static async (string host, int rotation) =>
{
    using var client = new DeviceClient(host);
    var result = await client.SetScreenRotationAngleAsync(rotation switch
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
    using var client = new DeviceClient(host);
    var result = await client.SetMirrorModeAsync(true);
    result.EnsureSuccessStatus();
});
mirrorCommand.Add(mirrorOnCommand);

var mirrorOffCommand = new Command("off", "Mirror mode off");
mirrorOffCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DeviceClient(host);
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
    using var client = new DeviceClient(host);
    var result = await client.SetHighlightModeAsync(true);
    result.EnsureSuccessStatus();
});
highlightCommand.Add(highlightOnCommand);

var highlightOffCommand = new Command("off", "Highlight mode off");
highlightOffCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DeviceClient(host);
    var result = await client.SetHighlightModeAsync(false);
    result.EnsureSuccessStatus();
});
highlightCommand.Add(highlightOffCommand);

// TODO

//--------------------------------------------------------------------------------
// Run
//--------------------------------------------------------------------------------
var result = await rootCommand.InvokeAsync(args).ConfigureAwait(false);
#if DEBUG
Console.ReadLine();
#endif
return result;
