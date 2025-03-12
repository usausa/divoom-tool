using System.CommandLine;
using System.CommandLine.NamingConventionBinder;

using Divoom.Client;

// ReSharper disable UseObjectOrCollectionInitializer

var rootCommand = new RootCommand("Divoom client");

//--------------------------------------------------------------------------------
// Device
//--------------------------------------------------------------------------------
var deviceCommand = new Command("device", "Get lan device list");
deviceCommand.Handler = CommandHandler.Create(static async (IConsole console) =>
{
    using var serviceClient = new ServiceClient();
    var list = await serviceClient.FindDevices();

    foreach (var device in list)
    {
        console.WriteLine($"{device.Id} {device.MacAddress} {device.IpAddress} {device.Hardware} {device.Name}");
    }
});
rootCommand.Add(deviceCommand);

// TODO

//--------------------------------------------------------------------------------
// Run
//--------------------------------------------------------------------------------
var result = await rootCommand.InvokeAsync(args).ConfigureAwait(false);
#if DEBUG
Console.ReadLine();
#endif
return result;
