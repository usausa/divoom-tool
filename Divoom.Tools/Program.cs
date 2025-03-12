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
    var result = await client.GetCurrentChannelAsync();
    result.EnsureSuccessStatus();

    console.WriteLine($"{result.Index}");
});
rootCommand.Add(currentCommand);

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

// TODO

//--------------------------------------------------------------------------------
// Run
//--------------------------------------------------------------------------------
var result = await rootCommand.InvokeAsync(args).ConfigureAwait(false);
#if DEBUG
Console.ReadLine();
#endif
return result;
