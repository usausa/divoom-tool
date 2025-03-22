using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Text.Json;

using Divoom.Client;
using Divoom.Tools;

using SkiaSharp;

// ReSharper disable UseObjectOrCollectionInitializer

var rootCommand = new RootCommand("Divoom client");

//--------------------------------------------------------------------------------
// device
//--------------------------------------------------------------------------------
var deviceCommand = new Command("device", "Get LAN device list");
deviceCommand.Handler = CommandHandler.Create(static async (IConsole console) =>
{
    var result = await DivoomClient.GetDeviceListAsync();
    result.EnsureSuccessStatus();

    foreach (var device in result.DeviceList)
    {
        console.WriteLine($"{device.Id} {device.MacAddress} {device.IpAddress} {device.Hardware} {device.Name}");
    }
});
rootCommand.Add(deviceCommand);

//--------------------------------------------------------------------------------
// font
//--------------------------------------------------------------------------------
var fontCommand = new Command("font", "Get supported font list");
fontCommand.Handler = CommandHandler.Create(static async (IConsole console) =>
{
    var result = await DivoomClient.GetFontListAsync();
    result.EnsureSuccessStatus();

    foreach (var font in result.FontList)
    {
        var scroll = font.Type == FontType.CanScroll ? "+" : "-";
        console.WriteLine($"{font.Id} {font.Width}/{font.Height} {scroll} {font.Name} {font.Chars}");
    }
});
rootCommand.Add(fontCommand);

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

    console.WriteLine(result.Indexes.Length > 0 ? $"Index: {String.Join(',', result.Indexes.Select(static x => (IndexType)x))}" : $"Index: {(IndexType)result.Index}");
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
    var result = await client.SetChannelIndexAsync(IndexType.Clock);
    result.EnsureSuccessStatus();
});
rootCommand.Add(clockCommand);

var clockTypeCommand = new Command("type", "Get clock type");
clockTypeCommand.Handler = CommandHandler.Create(static async (IConsole console) =>
{
    var result = await DivoomClient.GetClockTypeAsync();
    result.EnsureSuccessStatus();

    foreach (var type in result.TypeList)
    {
        console.WriteLine(type);
    }
});
clockCommand.Add(clockTypeCommand);

var clockListCommand = new Command("list", "Get clock list");
clockListCommand.AddOption(new Option<string>(["--type", "-t"], "Dial type") { IsRequired = true });
clockListCommand.AddOption(new Option<bool>(["--lcd", "-l"], "LCD"));
clockListCommand.AddOption(new Option<int>(["--page", "-p"], () => 1, "Page"));
clockListCommand.Handler = CommandHandler.Create(static async (IConsole console, string type, bool lcd, int page) =>
{
    var result = await DivoomClient.GeClockListAsync(type, lcd ? "LCD" : null, page);
    result.EnsureSuccessStatus();

    console.WriteLine($"Total: {result.Total}");
    foreach (var clock in result.ClockList)
    {
        console.WriteLine($"{clock.Id} {clock.Name}");
    }
});
clockCommand.Add(clockListCommand);

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
clockSelectCommand.AddOption(new Option<int>(["--clock", "-c"], "Clock id") { IsRequired = true });
clockSelectCommand.AddOption(new Option<int?>(["--lcd", "-l"], "Lcd independence id"));
clockSelectCommand.AddOption(new Option<int?>(["--index", "-i"], "Lcd index"));
clockSelectCommand.Handler = CommandHandler.Create(static async (string host, int clock, int? lcd, int? index) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SelectClockIdAsync(clock, lcd, index);
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
    var result = await client.SetChannelIndexAsync(IndexType.Cloud);
    result.EnsureSuccessStatus();
});
rootCommand.Add(cloudCommand);

var cloudSelectCommand = new Command("select", "Select cloud page");
cloudSelectCommand.AddOption(new Option<int>(["--page", "-p"], "Page index") { IsRequired = true });
cloudSelectCommand.AddOption(new Option<int?>(["--lcd", "-l"], "Lcd independence id") { IsRequired = true });
cloudSelectCommand.AddOption(new Option<int?>(["--index", "-i"], "Lcd index") { IsRequired = true });
cloudSelectCommand.Handler = CommandHandler.Create(static async (string host, int page, int? lcd, int? index) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SelectCloudIndexAsync((CloudIndex)page, lcd, index);
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
    var result = await client.SetChannelIndexAsync(IndexType.Equalizer);
    result.EnsureSuccessStatus();
});
rootCommand.Add(equalizerCommand);

var equalizerSelectCommand = new Command("select", "Select equalizer");
equalizerSelectCommand.AddOption(new Option<int>(["--pos", "-p"], "Equalizer position") { IsRequired = true });
equalizerSelectCommand.AddOption(new Option<int?>(["--lcd", "-l"], "Lcd independence id") { IsRequired = true });
equalizerSelectCommand.AddOption(new Option<int?>(["--index", "-i"], "Lcd index") { IsRequired = true });
equalizerSelectCommand.Handler = CommandHandler.Create(static async (string host, int pos, int? lcd, int? index) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SelectEqualizerIdAsync(pos, lcd, index);
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
    var result = await client.SetChannelIndexAsync(IndexType.Custom);
    result.EnsureSuccessStatus();
});
rootCommand.Add(customCommand);

var customSelectCommand = new Command("select", "Select custom page");
customSelectCommand.AddOption(new Option<int>(["--page", "-p"], "Page index") { IsRequired = true });
customSelectCommand.AddOption(new Option<int?>(["--lcd", "-l"], "Lcd independence id") { IsRequired = true });
customSelectCommand.AddOption(new Option<int?>(["--index", "-i"], "Lcd index") { IsRequired = true });
customSelectCommand.Handler = CommandHandler.Create(static async (string host, int page, int? lcd, int? index) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SelectCustomPageAsync(page, lcd, index);
    result.EnsureSuccessStatus();
});
customCommand.Add(customSelectCommand);

//--------------------------------------------------------------------------------
// lcd5
//--------------------------------------------------------------------------------
var lcd5Command = new Command("lcd5", "Get lcd whole information");
rootCommand.Add(lcd5Command);

var lcd5ListCommand = new Command("list", "Get lcd whole list");
lcd5ListCommand.AddOption(new Option<int>(["--page", "-p"], () => 1, "Page"));
lcd5ListCommand.Handler = CommandHandler.Create(static async (IConsole console, int page) =>
{
    var result = await DivoomClient.GetLcd5ClockListAsync(page);
    result.EnsureSuccessStatus();

    console.WriteLine($"Total: {result.Total}");
    foreach (var clock in result.ClockList)
    {
        console.WriteLine($"{clock.Id} {clock.Name}");
    }
});
lcd5Command.Add(lcd5ListCommand);

var lcd5InfoCommand = new Command("info", "Get lcd independence information");
lcd5InfoCommand.AddOption(new Option<int>(["--device", "-d"], "Device id") { IsRequired = true });
lcd5InfoCommand.Handler = CommandHandler.Create(static async (IConsole console, int device) =>
{
    var result = await DivoomClient.GetLcd5ClockInfoAsync(device, "LCD");
    result.EnsureSuccessStatus();

    var channelType = result.ChannelType == 1 ? "Independence" : "Whole";
    console.WriteLine($"ChannelType: {channelType}");
    console.WriteLine($"Independence: {result.Independence}");
    console.WriteLine($"ClockId: {result.ClockId}");
    for (var i = 0; i < result.LcdIndependenceList.Length; i++)
    {
        var independence = result.LcdIndependenceList[i];
        console.WriteLine($"{i + 1}: {independence.Independence} {independence.Name}");
        foreach (var lcd in independence.LcdList)
        {
            console.WriteLine($"{(IndexType)lcd.Index} {lcd.ClockId} {lcd.ImagePixelId}");
        }
    }
});
lcd5Command.Add(lcd5InfoCommand);

var lcd5ChannelCommand = new Command("channel", "Set channel type");
lcd5ChannelCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
lcd5ChannelCommand.AddOption(new Option<string>(["--type", "-t"], "Channel type") { IsRequired = true }.FromAmong("whole", "w", "independence", "i"));
lcd5ChannelCommand.AddOption(new Option<int?>(["--lcd", "-l"], "Lcd independence id"));
lcd5ChannelCommand.Handler = CommandHandler.Create(static async (string host, string type, int? lcd) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SetLcd5ChannelTypeAsync(
        type switch
        {
            "whole" or "w" => Lcd5ChannelType.Whole,
            _ => Lcd5ChannelType.Independence
        },
        lcd);
    result.EnsureSuccessStatus();
});
lcd5Command.Add(lcd5ChannelCommand);

var lcd5WholeCommand = new Command("whole", "Select whole clock");
lcd5WholeCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
lcd5WholeCommand.AddOption(new Option<int>(["--clock", "-c"], "Whole clock id"));
lcd5WholeCommand.Handler = CommandHandler.Create(static async (string host, int clock) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SelectLcd5WholeClockIdIdAsync(clock);
    result.EnsureSuccessStatus();
});
lcd5Command.Add(lcd5WholeCommand);

//--------------------------------------------------------------------------------
// monitor
//--------------------------------------------------------------------------------
var monitorCommand = new Command("monitor", "Monitor");
monitorCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
monitorCommand.AddOption(new Option<int?>(["--lcd", "-l"], "Lcd independence id") { IsRequired = true });
monitorCommand.AddOption(new Option<int?>(["--index", "-i"], "Lcd index") { IsRequired = true });
monitorCommand.Handler = CommandHandler.Create(static async (string host, int? lcd, int? index) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SelectClockIdAsync(625, lcd, index);
    result.EnsureSuccessStatus();
});
rootCommand.Add(monitorCommand);

var monitorUpdateCommand = new Command("update", "Update monitor");
monitorUpdateCommand.AddOption(new Option<string>(["--data", "-d"], () => string.Empty, "Data"));
monitorUpdateCommand.AddOption(new Option<string>(["--data1", "-d1"], () => string.Empty, "Data1"));
monitorUpdateCommand.AddOption(new Option<string>(["--data2", "-d2"], () => string.Empty, "Data2"));
monitorUpdateCommand.AddOption(new Option<string>(["--data3", "-d3"], () => string.Empty, "Data3"));
monitorUpdateCommand.AddOption(new Option<string>(["--data4", "-d4"], () => string.Empty, "Data4"));
monitorUpdateCommand.AddOption(new Option<string>(["--data5", "-d5"], () => string.Empty, "Data5"));
monitorUpdateCommand.Handler = CommandHandler.Create(static async (string host, string data, string data1, string data2, string data3, string data4, string data5) =>
{
    var list = new List<MonitorParameter>();
    AddParameter(list, null, data);
    AddParameter(list, 0, data1);
    AddParameter(list, 1, data2);
    AddParameter(list, 2, data3);
    AddParameter(list, 3, data4);
    AddParameter(list, 4, data5);

    using var client = new DivoomClient(host);
    var result = await client.UpdatePcMonitorAsync(list);
    result.EnsureSuccessStatus();

    void AddParameter(List<MonitorParameter> list, int? lcd, string data)
    {
        if (!String.IsNullOrEmpty(data))
        {
            var values = data.Split(',');
            list.Add(new MonitorParameter
            {
                Lcd = lcd,
                CpuUsed = values.ElementAtOrDefault(0) ?? string.Empty,
                GpuUsed = values.ElementAtOrDefault(1) ?? string.Empty,
                CpuTemperature = values.ElementAtOrDefault(2) ?? string.Empty,
                GpuTemperature = values.ElementAtOrDefault(3) ?? string.Empty,
                MemoryUsed = values.ElementAtOrDefault(4) ?? string.Empty,
                DiskTemperature = values.ElementAtOrDefault(5) ?? string.Empty
            });
        }
    }
});
monitorCommand.Add(monitorUpdateCommand);

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

var noiseStartCommand = new Command("start", "Start noise tool");
noiseStartCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.NoiseToolAsync(true);
    result.EnsureSuccessStatus();
});
noiseCommand.Add(noiseStartCommand);

var noiseStopCommand = new Command("stop", "Stop noise tool");
noiseStopCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.NoiseToolAsync(false);
    result.EnsureSuccessStatus();
});
noiseCommand.Add(noiseStopCommand);

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

    foreach (var image in result.ImageList)
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

    foreach (var image in result.ImageList)
    {
        console.WriteLine($"{image.FileName} {image.FileId}");
    }
});
remoteCommand.Add(remoteLikeCommand);

var remoteDrawCommand = new Command("draw", "Draw remote image");
remoteDrawCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
remoteDrawCommand.AddOption(new Option<string>(["--id", "-i"], "File id") { IsRequired = true });
remoteDrawCommand.AddOption(new Option<string>(["--array", "-a"], "Lcd array"));
remoteDrawCommand.Handler = CommandHandler.Create(static async (string host, string id, string array) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SendRemoteAsync(
        id,
        !String.IsNullOrEmpty(array) ? array.ToCharArray().Select(static x => Int32.Parse(x.ToString())).ToArray() : null);
    result.EnsureSuccessStatus();
});
remoteCommand.Add(remoteDrawCommand);

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
        alignment switch
        {
            "m" or "middle" => TextAlignment.Middle,
            "r" or "right" => TextAlignment.Right,
            _ => TextAlignment.Left
        },
        direction switch
        {
            "m" or "middle" => TextDirection.Right,
            _ => TextDirection.Left
        },
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
// display
//--------------------------------------------------------------------------------
var displayCommand = new Command("display", "Display item list");
displayCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
displayCommand.AddOption(new Option<string>(["--file", "-f"], "File") { IsRequired = true });
displayCommand.Handler = CommandHandler.Create(static async (string host, string file) =>
{
    var json = await File.ReadAllTextAsync(file);
    var list = JsonSerializer.Deserialize<DrawItem[]>(json);

    using var client = new DivoomClient(host);
    var result = await client.SendItemListAsync(list!);
    result.EnsureSuccessStatus();
});
rootCommand.Add(displayCommand);

//--------------------------------------------------------------------------------
// gif
//--------------------------------------------------------------------------------
var gifCommand = new Command("gif", "Gif animation");
gifCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
rootCommand.Add(gifCommand);

var gifPlayCommand = new Command("play", "Play gif image");
gifPlayCommand.AddOption(new Option<string>(["--type", "-t"], "File type") { IsRequired = true }.FromAmong("n", "net", "d", "directory", "f", "file"));
gifPlayCommand.AddOption(new Option<string>(["--name", "-n"], "File name") { IsRequired = true });
gifPlayCommand.Handler = CommandHandler.Create(static async (string host, string type, string name) =>
{
    using var client = new DivoomClient(host);
    var result = await client.PlayGifAsync(
        type switch
        {
            "d" or "directory" => PlayFileType.Folder,
            "f" or "file" => PlayFileType.File,
            _ => PlayFileType.Net
        },
        name);
    result.EnsureSuccessStatus();
});
gifCommand.Add(gifPlayCommand);

var gifArrayCommand = new Command("array", "Play gif array");
gifArrayCommand.AddOption(new Option<string>(["--array", "-a"], "Lcd array") { IsRequired = true });
gifArrayCommand.AddOption(new Option<string>(["--urls", "-u"], "File url") { IsRequired = true });
gifArrayCommand.Handler = CommandHandler.Create(static async (string host, string array, string urls) =>
{
    using var client = new DivoomClient(host);
    var result = await client.PlayGifArrayAsync(
        array.ToCharArray().Select(static x => Int32.Parse(x.ToString())).ToArray(),
        urls.Split(','));
    result.EnsureSuccessStatus();
});
gifCommand.Add(gifArrayCommand);

var gifAllCommand = new Command("all", "Play gif all lcd");
gifAllCommand.AddOption(new Option<string>(["--lcd1", "-l1"], "Lcd1 files") { IsRequired = true });
gifAllCommand.AddOption(new Option<string>(["--lcd2", "-l2"], "Lcd2 files") { IsRequired = true });
gifAllCommand.AddOption(new Option<string>(["--lcd3", "-l3"], "Lcd3 files") { IsRequired = true });
gifAllCommand.AddOption(new Option<string>(["--lcd4", "-l4"], "Lcd4 files") { IsRequired = true });
gifAllCommand.AddOption(new Option<string>(["--lcd5", "-l5"], "Lcd5 files") { IsRequired = true });
gifAllCommand.Handler = CommandHandler.Create(static async (string host, string lcd1, string lcd2, string lcd3, string lcd4, string lcd5) =>
{
    using var client = new DivoomClient(host);
    var result = await client.PlayGifAllLcdAsync(
        lcd1.Split(','),
        lcd2.Split(','),
        lcd3.Split(','),
        lcd4.Split(','),
        lcd5.Split(','));
    result.EnsureSuccessStatus();
});
gifCommand.Add(gifAllCommand);

//--------------------------------------------------------------------------------
// time
//--------------------------------------------------------------------------------
var timeCommand = new Command("time", "Get/Set device time");
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
var screenCommand = new Command("screen", "Set screen mode");
screenCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
screenCommand.AddOption(new Option<string>(["--mode", "-m"], () => "on", "Highlight mode").FromAmong("on", "off"));
screenCommand.Handler = CommandHandler.Create(static async (string host, string mode) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SwitchScreenAsync(mode == "on");
    result.EnsureSuccessStatus();
});
rootCommand.Add(screenCommand);

//--------------------------------------------------------------------------------
// brightness
//--------------------------------------------------------------------------------
var brightnessCommand = new Command("brightness", "Set brightness");
brightnessCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
brightnessCommand.AddOption(new Option<int>(["--brightness", "-b"], "Brightness") { IsRequired = true });
brightnessCommand.Handler = CommandHandler.Create(static async (string host, int brightness) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SetBrightnessAsync(brightness);
    result.EnsureSuccessStatus();
});
rootCommand.Add(brightnessCommand);

//--------------------------------------------------------------------------------
// rotation
//--------------------------------------------------------------------------------
var rotationCommand = new Command("rotation", "Set rotation");
rotationCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
rotationCommand.AddOption(new Option<int>(["--angle", "-a"], "Angle") { IsRequired = true });
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
mirrorCommand.AddOption(new Option<string>(["--mode", "-m"], () => "on", "Highlight mode").FromAmong("on", "off"));
mirrorCommand.Handler = CommandHandler.Create(static async (string host, string mode) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SetMirrorModeAsync(mode == "on");
    result.EnsureSuccessStatus();
});
rootCommand.Add(mirrorCommand);

//--------------------------------------------------------------------------------
// highlight
//--------------------------------------------------------------------------------
var highlightCommand = new Command("highlight", "Set highlight mode");
highlightCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
highlightCommand.AddOption(new Option<string>(["--mode", "-m"], () => "on", "Highlight mode").FromAmong("on", "off"));
highlightCommand.Handler = CommandHandler.Create(static async (string host, string mode) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SetHighlightModeAsync(mode == "on");
    result.EnsureSuccessStatus();
});
rootCommand.Add(highlightCommand);

//--------------------------------------------------------------------------------
// white
//--------------------------------------------------------------------------------
var whiteCommand = new Command("white", "Set white balance");
whiteCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
whiteCommand.AddOption(new Option<int>(["--red", "-r"], "Red") { IsRequired = true });
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
// rgb
//--------------------------------------------------------------------------------
var rgbCommand = new Command("rgb", "Set rgb mode");
rgbCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
rgbCommand.AddOption(new Option<int>(["--brightness", "-b"], () => 100, "Brightness"));
rgbCommand.AddOption(new Option<string>(["--color", "-c"], () => "#000000", "Color"));
rgbCommand.AddOption(new Option<string>(["--light", "-l"], () => "on", "Light switch").FromAmong("on", "off"));
rgbCommand.AddOption(new Option<string>(["--key", "-k"], () => "on", "Keyboard light").FromAmong("on", "off"));
rgbCommand.AddOption(new Option<string>(["--cycle", "-y"], () => "on", "Color cycle").FromAmong("on", "off"));
rgbCommand.AddOption(new Option<string>(["--index", "-i"], () => "all", "Light index").FromAmong("all", "edge", "back"));
rgbCommand.AddOption(new Option<string>(["--effect", "-e"], () => "0", "Color cycle"));
rgbCommand.Handler = CommandHandler.Create(static async (string host, int brightness, string color, string light, string key, string cycle, string index, string effect) =>
{
    using var client = new DivoomClient(host);
    var result = await client.SetRgbInformationAsync(
        brightness,
        color.StartsWith('#') ? color : "#" + color,
        light == "on",
        key == "on",
        cycle == "on",
        index switch
        {
            "edge" => LightIndex.Edge,
            "back" => LightIndex.Backlight,
            _ => LightIndex.All
        },
        effect.Split(',').Select(Int32.Parse));
    result.EnsureSuccessStatus();
});
rootCommand.Add(rgbCommand);

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

    Show(console, "Weather", result.Weather);
    Show(console, "Brightness", result.Brightness);
    Show(console, "Rotation", result.Rotation, static x => (RotationAngle)x! switch
    {
        RotationAngle.Rotate90 => 90,
        RotationAngle.Rotate180 => 180,
        RotationAngle.Rotate270 => 270,
        _ => 0
    });
    Show(console, "ClockTime", result.ClockTime);
    Show(console, "GalleryTime", result.GalleryTime);
    Show(console, "SingleGalleyTime", result.SingleGalleyTime);
    Show(console, "PowerOnChannelId", result.PowerOnChannelId);
    Show(console, "GalleryShowTime", result.GalleryShowTime);
    Show(console, "CurrentClockId", result.CurrentClockId);
    Show(console, "DateFormat", result.DateFormat);
    Show(console, "HourMode", result.Time24, static x => (HourMode)x == HourMode.Hour12 ? "12" : "24");
    Show(console, "TemperatureMode", result.TemperatureMode, static x => (TemperatureMode)x);
    Show(console, "GyrateAngle", result.GyrateAngle);
    Show(console, "Mirror", result.Mirror, static x => x == 1 ? "enable" : "disable");
    Show(console, "LightSwitch", result.LightSwitch, static x => x == 1 ? "on" : "off");

    void Show<T>(IConsole console, string name, T? value, Func<T, object>? converter = null)
    {
        if (value is not null)
        {
            console.WriteLine(converter is not null ? $"{name}: {converter(value)}" : $"{name}: {value}");
        }
    }
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
timezoneCommand.AddOption(new Option<string>(["--zone", "-z"], "Timezone") { IsRequired = true });
timezoneCommand.Handler = CommandHandler.Create(static async (string host, string zone) =>
{
    using var client = new DivoomClient(host);
    var result = await client.ConfigTimeZoneAsync(zone);
    result.EnsureSuccessStatus();
});
rootCommand.Add(timezoneCommand);

//--------------------------------------------------------------------------------
// temperature
//--------------------------------------------------------------------------------
var temperatureCommand = new Command("temperature", "Set temperature mode");
temperatureCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
temperatureCommand.AddOption(new Option<string>(["--mode", "-m"], "Mode") { IsRequired = true }.FromAmong("c", "celsius", "f", "fahrenheit"));
temperatureCommand.Handler = CommandHandler.Create(static async (string host, string mode) =>
{
    using var client = new DivoomClient(host);
    var result = await client.ConfigTemperatureModeAsync(
        mode switch
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
hourCommand.Handler = CommandHandler.Create(static async (string host, string mode) =>
{
    using var client = new DivoomClient(host);
    var result = await client.ConfigHourModeAsync(
        mode switch
        {
            "24" => HourMode.Hour24,
            _ => HourMode.Hour12
        });
    result.EnsureSuccessStatus();
});
rootCommand.Add(hourCommand);

//--------------------------------------------------------------------------------
// reboot
//--------------------------------------------------------------------------------
var rebootCommand = new Command("reboot", "Reboot");
rebootCommand.AddGlobalOption(new Option<string>(["--host", "-h"], "Host") { IsRequired = true });
rebootCommand.Handler = CommandHandler.Create(static async (string host) =>
{
    using var client = new DivoomClient(host);
    var result = await client.RebootAsync();
    result.EnsureSuccessStatus();
});
rootCommand.Add(rebootCommand);

//--------------------------------------------------------------------------------
// Run
//--------------------------------------------------------------------------------
var result = await rootCommand.InvokeAsync(args).ConfigureAwait(false);
#if DEBUG
Console.ReadLine();
#endif
return result;
