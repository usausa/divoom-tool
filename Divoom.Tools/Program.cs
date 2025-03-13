using System;
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

    console.WriteLine($"{result.Index}");
});
rootCommand.Add(currentCommand);

//--------------------------------------------------------------------------------
// clock
//--------------------------------------------------------------------------------
var clockCommand = new Command("clock", "Get clock channel");
clockCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
clockCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DeviceClient(host);
    var result = await client.SetChannelIndexAsync(Channel.Clock);
    result.EnsureSuccessStatus();
});
rootCommand.Add(clockCommand);

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
    var result = await client.SetStopWatchAsync(StopWatchCommand.Start);
    result.EnsureSuccessStatus();
});
watchCommand.Add(watchStartCommand);

var watchStopCommand = new Command("stop", "Stopwatch stop");
watchStopCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DeviceClient(host);
    var result = await client.SetStopWatchAsync(StopWatchCommand.Stop);
    result.EnsureSuccessStatus();
});
watchCommand.Add(watchStopCommand);

var watchResetCommand = new Command("reset", "Stopwatch reset");
watchResetCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DeviceClient(host);
    var result = await client.SetStopWatchAsync(StopWatchCommand.Reset);
    result.EnsureSuccessStatus();
});
watchCommand.Add(watchResetCommand);

// TODO

//--------------------------------------------------------------------------------
// Run
//--------------------------------------------------------------------------------
var result = await rootCommand.InvokeAsync(args).ConfigureAwait(false);
#if DEBUG
Console.ReadLine();
#endif
return result;
