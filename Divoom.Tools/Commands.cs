// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Divoom.Tools;

using System.Text.Json;

using Divoom.Client;

using SkiaSharp;

using Smart.CommandLine.Hosting;

public static class CommandBuilderExtensions
{
    public static void AddCommands(this ICommandBuilder commands)
    {
        commands.AddCommand<DeviceCommand>();
        commands.AddCommand<FontCommand>();
        commands.AddCommand<CurrentCommand>();
        commands.AddCommand<ChannelCommand>();
        commands.AddCommand<Lcd5Command>(lcd5 =>
        {
            lcd5.AddSubCommand<Lcd5ListCommand>();
            lcd5.AddSubCommand<Lcd5InfoCommand>();
            lcd5.AddSubCommand<Lcd5ChannelCommand>();
            lcd5.AddSubCommand<Lcd5WholeCommand>();
        });
        commands.AddCommand<ClockCommand>(clock =>
        {
            clock.AddSubCommand<ClockTypeCommand>();
            clock.AddSubCommand<ClockListCommand>();
            clock.AddSubCommand<ClockInfoCommand>();
            clock.AddSubCommand<ClockSelectCommand>();
        });
        commands.AddCommand<CloudCommand>(cloud =>
        {
            cloud.AddSubCommand<CloudSelectCommand>();
        });
        commands.AddCommand<EqualizerCommand>(equalizer =>
        {
            equalizer.AddSubCommand<EqualizerSelectCommand>();
        });
        commands.AddCommand<CustomCommand>(custom =>
        {
            custom.AddSubCommand<CustomSelectCommand>();
        });
        commands.AddCommand<MonitorCommand>(monitor =>
        {
            monitor.AddSubCommand<MonitorSelectCommand>();
            monitor.AddSubCommand<MonitorUpdateCommand>();
        });
        commands.AddCommand<TimerCommand>(timer =>
        {
            timer.AddSubCommand<TimerStartCommand>();
            timer.AddSubCommand<TimerStopCommand>();
        });
        commands.AddCommand<WatchCommand>(watch =>
        {
            watch.AddSubCommand<WatchStartCommand>();
            watch.AddSubCommand<WatchStopCommand>();
            watch.AddSubCommand<WatchResetCommand>();
        });
        commands.AddCommand<ScoreCommand>();
        commands.AddCommand<NoiseCommand>(noise =>
        {
            noise.AddSubCommand<NoiseStartCommand>();
            noise.AddSubCommand<NoiseStopCommand>();
        });
        commands.AddCommand<ImageCommand>(image =>
        {
            image.AddSubCommand<ImageResetCommand>();
            image.AddSubCommand<ImageIdCommand>();
            image.AddSubCommand<ImageDrawCommand>();
            image.AddSubCommand<ImageFillCommand>();
        });
        commands.AddCommand<RemoteCommand>(remote =>
        {
            remote.AddSubCommand<RemoteListCommand>();
            remote.AddSubCommand<RemoteLikeCommand>();
            remote.AddSubCommand<RemoteDrawCommand>();
        });
        commands.AddCommand<TextCommand>(text =>
        {
            text.AddSubCommand<TextDrawCommand>();
            text.AddSubCommand<TextClearCommand>();
        });
        commands.AddCommand<DisplayCommand>();
        commands.AddCommand<GifCommand>(gif =>
        {
            gif.AddSubCommand<GifPlayCommand>();
            gif.AddSubCommand<GifArrayCommand>();
            gif.AddSubCommand<GifAllCommand>();
        });
        commands.AddCommand<TimeCommand>();
        commands.AddCommand<WeatherCommand>();
        commands.AddCommand<BuzzerCommand>();
        commands.AddCommand<ScreenCommand>();
        commands.AddCommand<BrightnessCommand>();
        commands.AddCommand<RotationCommand>();
        commands.AddCommand<MirrorCommand>();
        commands.AddCommand<HighlightCommand>();
        commands.AddCommand<WhiteCommand>();
        commands.AddCommand<RgbCommand>();
        commands.AddCommand<ConfigCommand>();
        commands.AddCommand<AreaCommand>();
        commands.AddCommand<TimezoneCommand>();
        commands.AddCommand<TemperatureCommand>();
        commands.AddCommand<HourCommand>();
        commands.AddCommand<RebootCommand>();
    }
}

public abstract class BaseHostCommand
{
    [Option<string>("--host", "-h", Description = "Host", Required = true)]
    public string Host { get; set; } = default!;
}

//--------------------------------------------------------------------------------
// device
//--------------------------------------------------------------------------------
[Command("device", "Get LAN device list")]
public sealed class DeviceCommand : ICommandHandler
{
    public async ValueTask ExecuteAsync(CommandContext context)
    {
        var result = await DivoomClient.GetDeviceListAsync();
        result.EnsureSuccessStatus();

        foreach (var device in result.DeviceList)
        {
            Console.WriteLine($"{device.Id} {device.MacAddress} {device.IpAddress} {device.Hardware} {device.Name}");
        }
    }
}

//--------------------------------------------------------------------------------
// font
//--------------------------------------------------------------------------------
[Command("font", "Get supported font list")]
public sealed class FontCommand : ICommandHandler
{
    public async ValueTask ExecuteAsync(CommandContext context)
    {
        var result = await DivoomClient.GetFontListAsync();
        result.EnsureSuccessStatus();

        foreach (var font in result.FontList)
        {
            var scroll = font.Type == FontType.CanScroll ? "+" : "-";
            Console.WriteLine($"{font.Id} {font.Width}/{font.Height} {scroll} {font.Name} {font.Chars}");
        }
    }
}

//--------------------------------------------------------------------------------
// current
//--------------------------------------------------------------------------------
[Command("current", "Get current channel")]
public sealed class CurrentCommand : BaseHostCommand, ICommandHandler
{
    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.GetChannelIndexAsync();
        result.EnsureSuccessStatus();

        Console.WriteLine(result.Indexes.Length > 0 ? $"Index: {String.Join(',', result.Indexes.Select(static x => (IndexType)x))}" : $"Index: {(IndexType)result.Index}");
    }
}

//--------------------------------------------------------------------------------
// channel
//--------------------------------------------------------------------------------
[Command("channel", "Set channel type")]
public sealed class ChannelCommand : BaseHostCommand, ICommandHandler
{
    [Option<string>("--type", "-t", Description = "Channel type", Required = true, Completions = ["clock", "cloud", "equalizer", "custom"])]
    public string Type { get; set; } = default!;

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.SetChannelIndexAsync(
            Type switch
            {
                "cloud" => IndexType.Cloud,
                "equalizer" => IndexType.Equalizer,
                "custom" => IndexType.Custom,
                _ => IndexType.Clock
            });
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// lcd5
//--------------------------------------------------------------------------------
[Command("lcd5", "Get lcd whole information")]
public sealed class Lcd5Command
{
}

[Command("list", "Get lcd whole list")]
public sealed class Lcd5ListCommand : ICommandHandler
{
    [Option<int>("--page", "-p", Description = "Page", DefaultValue = 1)]
    public int Page { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        var result = await DivoomClient.GetLcd5ClockListAsync(Page);
        result.EnsureSuccessStatus();

        Console.WriteLine($"Total: {result.Total}");
        foreach (var clock in result.ClockList)
        {
            Console.WriteLine($"{clock.Id} {clock.Name}");
        }
    }
}

[Command("info", "Get lcd independence information")]
public sealed class Lcd5InfoCommand : ICommandHandler
{
    [Option<string>("--device", "-d", Description = "Device id", Required = true)]
    public int Device { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        var result = await DivoomClient.GetLcd5ClockInfoAsync(Device, "LCD");
        result.EnsureSuccessStatus();

        var channelType = result.ChannelType == 1 ? "Independence" : "Whole";
        Console.WriteLine($"ChannelType: {channelType}");
        Console.WriteLine($"Independence: {result.Independence}");
        Console.WriteLine($"ClockId: {result.ClockId}");
        for (var i = 0; i < result.LcdIndependenceList.Length; i++)
        {
            var independence = result.LcdIndependenceList[i];
            Console.WriteLine($"{i + 1}: {independence.Independence} {independence.Name}");
            foreach (var lcd in independence.LcdList)
            {
                Console.WriteLine($"{(IndexType)lcd.Index} {lcd.ClockId} {lcd.ImagePixelId}");
            }
        }
    }
}

[Command("channel", "Set channel type")]
public sealed class Lcd5ChannelCommand : BaseHostCommand, ICommandHandler
{
    [Option<string>("--type", "-t", Description = "Channel type", Required = true, Completions = ["whole", "w", "independence", "i"])]
    public string Type { get; set; } = default!;

    [Option<int?>("--lcd", "-l", Description = "Lcd independence id")]
    public int? Lcd { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.SetLcd5ChannelTypeAsync(
            Type switch
            {
                "whole" or "w" => Lcd5ChannelType.Whole,
                _ => Lcd5ChannelType.Independence
            },
            Lcd);
        result.EnsureSuccessStatus();
    }
}

[Command("whole", "Select whole clock")]
public sealed class Lcd5WholeCommand : BaseHostCommand, ICommandHandler
{
    [Option<int>("--clock", "-c", Description = "Whole clock id", Required = true)]
    public int Clock { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.SelectLcd5WholeClockIdIdAsync(Clock);
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// clock
//--------------------------------------------------------------------------------
[Command("clock", "Clock channel")]
public sealed class ClockCommand
{
}

[Command("type", "Get clock type")]
public sealed class ClockTypeCommand : ICommandHandler
{
    public async ValueTask ExecuteAsync(CommandContext context)
    {
        var result = await DivoomClient.GetClockTypeAsync();
        result.EnsureSuccessStatus();

        foreach (var type in result.TypeList)
        {
            Console.WriteLine(type);
        }
    }
}

[Command("list", "Get clock list")]
public sealed class ClockListCommand : ICommandHandler
{
    [Option<string>("--type", "-t", Description = "Dial type", Required = true)]
    public string Type { get; set; } = default!;

    [Option<bool>("--lcd", "-l", Description = "LCD")]
    public bool Lcd { get; set; }

    [Option<int>("--page", "-p", Description = "Page", DefaultValue = 1)]
    public int Page { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        var result = await DivoomClient.GeClockListAsync(Type, Lcd ? "LCD" : null, Page);
        result.EnsureSuccessStatus();

        Console.WriteLine($"Total: {result.Total}");
        foreach (var clock in result.ClockList)
        {
            Console.WriteLine($"{clock.Id} {clock.Name}");
        }
    }
}

[Command("info", "Show clock information")]
public sealed class ClockInfoCommand : BaseHostCommand, ICommandHandler
{
    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.GetClockInfoAsync();
        result.EnsureSuccessStatus();

        Console.WriteLine($"ClockId: {result.ClockId}");
        Console.WriteLine($"Brightness: {result.Brightness}");
    }
}

[Command("select", "Select clock")]
public sealed class ClockSelectCommand : BaseHostCommand, ICommandHandler
{
    [Option<int>("--clock", "-c", Description = "Clock id", Required = true)]
    public int Clock { get; set; }

    [Option<int?>("--lcd", "-l", Description = "Lcd independence id")]
    public int? Lcd { get; set; }

    [Option<int?>("--index", "-i", Description = "Lcd index")]
    public int? Index { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.SelectClockIdAsync(Clock, Lcd, Index);
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// cloud
//--------------------------------------------------------------------------------
[Command("cloud", "Cloud channel")]
public sealed class CloudCommand
{
}

[Command("select", "Select cloud page")]
public sealed class CloudSelectCommand : BaseHostCommand, ICommandHandler
{
    [Option<int>("--page", "-p", Description = "Page index", Required = true)]
    public int Page { get; set; }

    [Option<int?>("--lcd", "-l", Description = "Lcd independence id")]
    public int? Lcd { get; set; }

    [Option<int?>("--index", "-i", Description = "Lcd index")]
    public int? Index { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.SelectCloudIndexAsync((CloudIndex)Page, Lcd, Index);
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// equalizer
//--------------------------------------------------------------------------------
[Command("equalizer", "Equalizer channel")]
public sealed class EqualizerCommand
{
}

[Command("select", "Select equalizer")]
public sealed class EqualizerSelectCommand : BaseHostCommand, ICommandHandler
{
    [Option<int>("--pos", "-p", Description = "Equalizer position", Required = true)]
    public int Pos { get; set; }

    [Option<int?>("--lcd", "-l", Description = "Lcd independence id")]
    public int? Lcd { get; set; }

    [Option<int?>("--index", "-i", Description = "Lcd index")]
    public int? Index { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.SelectEqualizerIdAsync(Pos, Lcd, Index);
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// custom
//--------------------------------------------------------------------------------
[Command("custom", "Custom channel")]
public sealed class CustomCommand
{
}

[Command("select", "Select custom page")]
public sealed class CustomSelectCommand : BaseHostCommand, ICommandHandler
{
    [Option<int>("--page", "-p", Description = "Page index", Required = true)]
    public int Page { get; set; }

    [Option<int?>("--lcd", "-l", Description = "Lcd independence id")]
    public int? Lcd { get; set; }

    [Option<int?>("--index", "-i", Description = "Lcd index")]
    public int? Index { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.SelectCustomPageAsync(Page, Lcd, Index);
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// monitor
//--------------------------------------------------------------------------------
[Command("monitor", "Monitor")]
public sealed class MonitorCommand
{
}

[Command("select", "Select monitor")]
public sealed class MonitorSelectCommand : BaseHostCommand, ICommandHandler
{
    [Option<int?>("--lcd", "-l", Description = "Lcd independence id")]
    public int? Lcd { get; set; }

    [Option<int?>("--index", "-i", Description = "Lcd index")]
    public int? Index { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.SelectClockIdAsync(625, Lcd, Index);
        result.EnsureSuccessStatus();
    }
}

[Command("update", "Update monitor")]
public sealed class MonitorUpdateCommand : BaseHostCommand, ICommandHandler
{
    [Option<string>("--data", "-d", Description = "Data", DefaultValue = "")]
    public string Data { get; set; } = default!;

    [Option<string>("--data1", "-d1", Description = "Data1", DefaultValue = "")]
    public string Data1 { get; set; } = default!;

    [Option<string>("--data2", "-d2", Description = "Data2", DefaultValue = "")]
    public string Data2 { get; set; } = default!;

    [Option<string>("--data3", "-d3", Description = "Data3", DefaultValue = "")]
    public string Data3 { get; set; } = default!;

    [Option<string>("--data4", "-d4", Description = "Data4", DefaultValue = "")]
    public string Data4 { get; set; } = default!;

    [Option<string>("--data5", "-d5", Description = "Data5", DefaultValue = "")]
    public string Data5 { get; set; } = default!;

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        var list = new List<MonitorParameter>();
        AddParameter(list, null, Data);
        AddParameter(list, 0, Data1);
        AddParameter(list, 1, Data2);
        AddParameter(list, 2, Data3);
        AddParameter(list, 3, Data4);
        AddParameter(list, 4, Data5);

        using var client = new DivoomClient(Host);
        var result = await client.UpdatePcMonitorAsync(list);
        result.EnsureSuccessStatus();

        static void AddParameter(List<MonitorParameter> list, int? lcd, string data)
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
    }
}

//--------------------------------------------------------------------------------
// timer
//--------------------------------------------------------------------------------
[Command("timer", "Timer tool")]
public sealed class TimerCommand
{
}

[Command("start", "Start timer")]
public sealed class TimerStartCommand : BaseHostCommand, ICommandHandler
{
    [Option<int>("--second", "-s", Description = "Second", Required = true)]
    public int Second { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.TimerToolAsync(true, Second);
        result.EnsureSuccessStatus();
    }
}

[Command("stop", "Stop timer")]
public sealed class TimerStopCommand : BaseHostCommand, ICommandHandler
{
    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.TimerToolAsync(false, 0);
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// watch
//--------------------------------------------------------------------------------
[Command("watch", "Stopwatch tool")]
public sealed class WatchCommand
{
}

[Command("start", "Start stopwatch")]
public sealed class WatchStartCommand : BaseHostCommand, ICommandHandler
{
    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.StopwatchToolAsync(StopwatchCommand.Start);
        result.EnsureSuccessStatus();
    }
}

[Command("stop", "Stop stopwatch")]
public sealed class WatchStopCommand : BaseHostCommand, ICommandHandler
{
    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.StopwatchToolAsync(StopwatchCommand.Stop);
        result.EnsureSuccessStatus();
    }
}

[Command("reset", "Reset stopwatch")]
public sealed class WatchResetCommand : BaseHostCommand, ICommandHandler
{
    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.StopwatchToolAsync(StopwatchCommand.Reset);
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// score
//--------------------------------------------------------------------------------

[Command("score", "Scoreboard tool")]
public sealed class ScoreCommand : BaseHostCommand, ICommandHandler
{
    [Option<int>("--blue", "-b", Description = "Blue score", Required = true)]
    public int Blue { get; set; }

    [Option<int>("--red", "-r", Description = "Red score", Required = true)]
    public int Red { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.ScoreboardToolAsync(Blue, Red);
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// noise
//--------------------------------------------------------------------------------
[Command("noise", "Noise status tool")]
public sealed class NoiseCommand
{
}

[Command("start", "Start noise tool")]
public sealed class NoiseStartCommand : BaseHostCommand, ICommandHandler
{
    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.NoiseToolAsync(true);
        result.EnsureSuccessStatus();
    }
}

[Command("stop", "Stop noise tool")]
public sealed class NoiseStopCommand : BaseHostCommand, ICommandHandler
{
    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.NoiseToolAsync(false);
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// image
//--------------------------------------------------------------------------------
[Command("image", "Image tool")]
public sealed class ImageCommand
{
}

[Command("reset", "Reset image id")]
public sealed class ImageResetCommand : BaseHostCommand, ICommandHandler
{
    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.ResetPictureIdAsync();
        result.EnsureSuccessStatus();
    }
}

[Command("id", "Get image id")]
public sealed class ImageIdCommand : BaseHostCommand, ICommandHandler
{
    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.GetPictureIdAsync();
        result.EnsureSuccessStatus();

        Console.WriteLine($"Id: {result.PictureId}");
    }
}

[Command("draw", "Draw image")]
public sealed class ImageDrawCommand : BaseHostCommand, ICommandHandler
{
    [Option<int?>("--id", "-i", Description = "Id")]
    public int? Id { get; set; }

    [Option<string>("--file", "-f", Description = "File", Required = true)]
    public string File { get; set; } = default!;

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);

        if (!Id.HasValue)
        {
            var idResult = await client.GetPictureIdAsync();
            idResult.EnsureSuccessStatus();
            Id = idResult.PictureId;
        }

        await using var stream = System.IO.File.OpenRead(File);
        using var bitmap = SKBitmap.Decode(stream);
        var buffer = new byte[bitmap.Width * bitmap.Height * 3];
        for (var y = 0; y < bitmap.Height; y++)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                var span = buffer.AsSpan(((y * bitmap.Width) + x) * 3);
                var c = bitmap.GetPixel(x, y);
                span[0] = c.Red;
                span[1] = c.Green;
                span[2] = c.Blue;
            }
        }
        var data = Convert.ToBase64String(buffer);

        var result = await client.SendImageAsync(Id.Value, bitmap.Width, data);
        result.EnsureSuccessStatus();
    }
}

[Command("fill", "Fill image")]
public sealed class ImageFillCommand : BaseHostCommand, ICommandHandler
{
    [Option<int?>("--id", "-i", Description = "Id")]
    public int? Id { get; set; }

    [Option<int>("--size", "-s", Description = "Size", DefaultValue = 64)]
    public int Size { get; set; }

    [Option<string>("--color", "-c", Description = "Color", DefaultValue = "#000000")]
    public string Color { get; set; } = default!;

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        var c = SKColor.Parse(Color);

        using var client = new DivoomClient(Host);

        if (!Id.HasValue)
        {
            var idResult = await client.GetPictureIdAsync();
            idResult.EnsureSuccessStatus();
            Id = idResult.PictureId;
        }

        var buffer = new byte[Size * Size * 3];
        for (var offset = 0; offset < buffer.Length; offset += 3)
        {
            var span = buffer.AsSpan(offset);
            span[0] = c.Red;
            span[1] = c.Green;
            span[2] = c.Blue;
        }
        var data = Convert.ToBase64String(buffer);

        var result = await client.SendImageAsync(Id.Value, Size, data);
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// remote
//--------------------------------------------------------------------------------
[Command("remote", "Remote image")]
public sealed class RemoteCommand
{
}

[Command("list", "List upload images")]
public sealed class RemoteListCommand : ICommandHandler
{
    [Option<int>("--device", "-d", Description = "Device id", Required = true)]
    public int Device { get; set; }

    [Option<string>("--mac", "-m", Description = "Device mac", Required = true)]
    public string Mac { get; set; } = default!;

    [Option<int>("--page", "-p", Description = "Page", DefaultValue = 1)]
    public int Page { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        var result = await DivoomClient.GetUploadImageListAsync(Device, Mac, Page);
        result.EnsureSuccessStatus();

        foreach (var image in result.ImageList)
        {
            Console.WriteLine($"{image.FileName} {image.FileId}");
        }
    }
}

[Command("like", "List like images")]
public sealed class RemoteLikeCommand : ICommandHandler
{
    [Option<int>("--device", "-d", Description = "Device id", Required = true)]
    public int Device { get; set; }

    [Option<string>("--mac", "-m", Description = "Device mac", Required = true)]
    public string Mac { get; set; } = default!;

    [Option<int>("--page", "-p", Description = "Page", DefaultValue = 1)]
    public int Page { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        var result = await DivoomClient.GetLikeImageListAsync(Device, Mac, Page);
        result.EnsureSuccessStatus();

        foreach (var image in result.ImageList)
        {
            Console.WriteLine($"{image.FileName} {image.FileId}");
        }
    }
}

[Command("draw", "Draw remote image")]
public sealed class RemoteDrawCommand : BaseHostCommand, ICommandHandler
{
    [Option<string>("--id", "-i", Description = "File id", Required = true)]
    public string Id { get; set; } = default!;

    [Option<string>("--array", "-a", Description = "Lcd array")]
    public string Array { get; set; } = default!;

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.SendRemoteAsync(
            Id,
            !String.IsNullOrEmpty(Array) ? Array.ToCharArray().Select(static x => Int32.Parse(x.ToString())).ToArray() : null);
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// text
//--------------------------------------------------------------------------------
[Command("text", "Text tool")]
public sealed class TextCommand
{
}

[Command("draw", "Draw text")]
public sealed class TextDrawCommand : BaseHostCommand, ICommandHandler
{
    [Option<int>("--id", "-i", Description = "Id", Required = true)]
    public int Id { get; set; }

    [Option<int>("-x", Description = "Start x", Required = true)]
    public int X { get; set; }

    [Option<int>("-y", Description = "Start y", Required = true)]
    public int Y { get; set; }

    [Option<int>("--width", "-w", Description = "Text area width", Required = true)]
    public int Width { get; set; }

    [Option<int>("--font", "-f", Description = "Font id", Required = true)]
    public int Font { get; set; }

    [Option<string>("--color", "-c", Description = "Font color", Required = true)]
    public string Color { get; set; } = default!;

    [Option<string>("--text", "-t", Description = "Text string", Required = true)]
    public string Text { get; set; } = default!;

    [Option<string>("--alignment", "-a", Description = "Text alignment", DefaultValue = "l", Completions = ["l", "left", "m", "middle", "r", "right"])]
    public string Alignment { get; set; } = default!;

    [Option<string>("--direction", "-d", Description = "Scroll direction", DefaultValue = "l", Completions = ["l", "left", "r", "right"])]
    public string Direction { get; set; } = default!;

    [Option<int>("--speed", "-s", Description = "Scroll speed", DefaultValue = 0)]
    public int Speed { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.SendTextAsync(
            Id,
            X,
            Y,
            Width,
            Font,
            Color.StartsWith('#') ? Color : "#" + Color,
            Text,
            Alignment switch
            {
                "m" or "middle" => TextAlignment.Middle,
                "r" or "right" => TextAlignment.Right,
                _ => TextAlignment.Left
            },
            Direction switch
            {
                "m" or "middle" => TextDirection.Right,
                _ => TextDirection.Left
            },
            Speed);
        result.EnsureSuccessStatus();
    }
}

[Command("clear", "Clear text")]
public sealed class TextClearCommand : BaseHostCommand, ICommandHandler
{
    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.ClearTextAsync();
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// display
//--------------------------------------------------------------------------------
[Command("display", "Display item list")]
public sealed class DisplayCommand : BaseHostCommand, ICommandHandler
{
    [Option<string>("--file", "-f", Description = "File", Required = true)]
    public string File { get; set; } = default!;

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        var json = await System.IO.File.ReadAllTextAsync(File);
        var list = JsonSerializer.Deserialize<DrawItem[]>(json);

        using var client = new DivoomClient(Host);
        var result = await client.SendItemListAsync(list!);
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// gif
//--------------------------------------------------------------------------------
[Command("gif", "Gif animation")]
public sealed class GifCommand
{
}

[Command("play", "Play gif image")]
public sealed class GifPlayCommand : BaseHostCommand, ICommandHandler
{
    [Option<string>("--type", "-t", Description = "File type", Required = true, Completions = ["n", "net", "d", "directory", "f", "file"])]
    public string Type { get; set; } = default!;

    [Option<string>("--name", "-n", Description = "File name", Required = true)]
    public string Name { get; set; } = default!;

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.PlayGifAsync(
            Type switch
            {
                "d" or "directory" => PlayFileType.Folder,
                "f" or "file" => PlayFileType.File,
                _ => PlayFileType.Net
            },
            Name);
        result.EnsureSuccessStatus();
    }
}

[Command("array", "Play gif array")]
public sealed class GifArrayCommand : BaseHostCommand, ICommandHandler
{
    [Option<string>("--array", "-a", Description = "Lcd array", Required = true)]
    public string Array { get; set; } = default!;

    [Option<string>("--urls", "-u", Description = "File url", Required = true)]
    public string Urls { get; set; } = default!;

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.PlayGifArrayAsync(
            Array.ToCharArray().Select(static x => Int32.Parse(x.ToString())).ToArray(),
            Urls.Split(','));
        result.EnsureSuccessStatus();
    }
}

[Command("all", "Play gif all lcd")]
public sealed class GifAllCommand : BaseHostCommand, ICommandHandler
{
    [Option<string>("--lcd1", "-l1", Description = "Lcd1 files", Required = true)]
    public string Lcd1 { get; set; } = default!;

    [Option<string>("--lcd2", "-l2", Description = "Lcd2 files", Required = true)]
    public string Lcd2 { get; set; } = default!;

    [Option<string>("--lcd3", "-l3", Description = "Lcd3 files", Required = true)]
    public string Lcd3 { get; set; } = default!;

    [Option<string>("--lcd4", "-l4", Description = "Lcd4 files", Required = true)]
    public string Lcd4 { get; set; } = default!;

    [Option<string>("--lcd5", "-l5", Description = "Lcd5 files", Required = true)]
    public string Lcd5 { get; set; } = default!;

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.PlayGifAllLcdAsync(
            Lcd1.Split(','),
            Lcd2.Split(','),
            Lcd3.Split(','),
            Lcd4.Split(','),
            Lcd5.Split(','));
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// time
//--------------------------------------------------------------------------------
[Command("time", "Get/Set device time")]
public sealed class TimeCommand : BaseHostCommand, ICommandHandler
{
    [Option<string>("--time", "-t", Description = "Local time")]
    public string Time { get; set; } = default!;

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        if (String.IsNullOrEmpty(Time))
        {
            var result = await client.GetDeviceTimeAsync();
            result.EnsureSuccessStatus();

            Console.WriteLine($"UTC: {result.Utc}");
            Console.WriteLine($"LocalTime: {result.LocalTime}");
        }
        else
        {
            var utc = Time == "auto" ? DateTimeOffset.UtcNow : DateTimeOffset.Parse(Time).ToUniversalTime();

            var result = await client.ConfigSystemTimeAsync(utc);
            result.EnsureSuccessStatus();
        }
    }
}

//--------------------------------------------------------------------------------
// weather
//--------------------------------------------------------------------------------
[Command("weather", "Get device weather")]
public sealed class WeatherCommand : BaseHostCommand, ICommandHandler
{
    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.GetWeatherInfoAsync();
        result.EnsureSuccessStatus();

        Console.WriteLine($"Weather: {result.Weather}");
        Console.WriteLine($"CurrentTemperature: {result.CurrentTemperature}");
        Console.WriteLine($"MinTemperature: {result.MinTemperature}");
        Console.WriteLine($"MaxTemperature: {result.MaxTemperature}");
        Console.WriteLine($"Pressure: {result.Pressure}");
        Console.WriteLine($"Humidity: {result.Humidity}");
        Console.WriteLine($"Visibility: {result.Visibility}");
        Console.WriteLine($"WindSpeed: {result.WindSpeed}");
    }
}

//--------------------------------------------------------------------------------
// buzzer
//--------------------------------------------------------------------------------
[Command("buzzer", "Play buzzer")]
public sealed class BuzzerCommand : BaseHostCommand, ICommandHandler
{
    [Option<int>("--active", "-a", Description = "Active time", DefaultValue = 500)]
    public int Active { get; set; }

    [Option<int>("--off", "-f", Description = "Off time", DefaultValue = 500)]
    public int Off { get; set; }

    [Option<int>("--total", "-t", Description = "Total time", DefaultValue = 3000)]
    public int Total { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.PlayBuzzerAsync(Active, Off, Total);
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// screen
//--------------------------------------------------------------------------------
[Command("screen", "Set screen mode")]
public sealed class ScreenCommand : BaseHostCommand, ICommandHandler
{
    [Option<string>("--mode", "-m", Description = "Highlight mode", DefaultValue = "on", Completions = ["on", "off"])]
    public string Mode { get; set; } = default!;

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.SwitchScreenAsync(Mode == "on");
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// brightness
//--------------------------------------------------------------------------------
[Command("brightness", "Set brightness")]
public sealed class BrightnessCommand : BaseHostCommand, ICommandHandler
{
    [Option<int>("--brightness", "-b", Description = "Brightness", Required = true)]
    public int Brightness { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.SetBrightnessAsync(Brightness);
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// rotation
//--------------------------------------------------------------------------------
[Command("rotation", "Set rotation")]
public sealed class RotationCommand : BaseHostCommand, ICommandHandler
{
    [Option<int>("--angle", "-a", Description = "Angle", Required = true, Completions = [0, 90, 180, 270])]
    public int Angle { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.SetScreenRotationAsync(
            Angle switch
            {
                90 => RotationAngle.Rotate90,
                180 => RotationAngle.Rotate180,
                270 => RotationAngle.Rotate270,
                _ => 0
            });
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// mirror
//--------------------------------------------------------------------------------
[Command("mirror", "Set mirror mode")]
public sealed class MirrorCommand : BaseHostCommand, ICommandHandler
{
    [Option<string>("--mode", "-m", Description = "Mode", DefaultValue = "on", Completions = ["on", "off"])]
    public string Mode { get; set; } = default!;

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.SetMirrorModeAsync(Mode == "on");
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// highlight
//--------------------------------------------------------------------------------
[Command("highlight", "Set highlight mode")]
public sealed class HighlightCommand : BaseHostCommand, ICommandHandler
{
    [Option<string>("--mode", "-m", Description = "Mode", DefaultValue = "on", Completions = ["on", "off"])]
    public string Mode { get; set; } = default!;

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.SetHighlightModeAsync(Mode == "on");
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// white
//--------------------------------------------------------------------------------
[Command("white", "Set white balance")]
public sealed class WhiteCommand : BaseHostCommand, ICommandHandler
{
    [Option<int>("--red", "-r", Description = "Red", Required = true)]
    public int Red { get; set; }

    [Option<int>("--green", "-g", Description = "Green", Required = true)]
    public int Green { get; set; }

    [Option<int>("--blue", "-b", Description = "Blue", Required = true)]
    public int Blue { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.SetWhiteBalanceAsync(Red, Green, Blue);
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// rgb
//--------------------------------------------------------------------------------
[Command("rgb", "Set rgb mode")]
public sealed class RgbCommand : BaseHostCommand, ICommandHandler
{
    [Option<int>("--brightness", "-b", Description = "Brightness", DefaultValue = 100)]
    public int Brightness { get; set; }

    [Option<string>("--color", "-c", Description = "Color", DefaultValue = "#000000")]
    public string Color { get; set; } = default!;

    [Option<string>("--light", "-l", Description = "Light switch", DefaultValue = "on", Completions = ["on", "off"])]
    public string Light { get; set; } = default!;

    [Option<string>("--light", "-l", Description = "Keyboard light", DefaultValue = "on", Completions = ["on", "off"])]
    public string Key { get; set; } = default!;

    [Option<string>("--cycle", "-y", Description = "Color cycle", DefaultValue = "on", Completions = ["on", "off"])]
    public string Cycle { get; set; } = default!;

    [Option<string>("--index", "-i", Description = "Light index", DefaultValue = "all", Completions = ["all", "edge", "back"])]
    public string Index { get; set; } = default!;

    [Option<string>("--effect", "-e", Description = "Color effect", DefaultValue = "0")]
    public string Effect { get; set; } = default!;

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.SetRgbInformationAsync(
            Brightness,
            Color.StartsWith('#') ? Color : "#" + Color,
            Light == "on",
            Key == "on",
            Cycle == "on",
            Index switch
            {
                "edge" => LightIndex.Edge,
                "back" => LightIndex.Backlight,
                _ => LightIndex.All
            },
            Effect.Split(',').Select(Int32.Parse));
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// config
//--------------------------------------------------------------------------------
[Command("config", "Get all config")]
public sealed class ConfigCommand : BaseHostCommand, ICommandHandler
{
    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.GetAllConfigAsync();
        result.EnsureSuccessStatus();

        Show("Weather", result.Weather);
        Show("Brightness", result.Brightness);
        Show("Rotation", result.Rotation, static x => (RotationAngle)x! switch
        {
            RotationAngle.Rotate90 => 90,
            RotationAngle.Rotate180 => 180,
            RotationAngle.Rotate270 => 270,
            _ => 0
        });
        Show("ClockTime", result.ClockTime);
        Show("GalleryTime", result.GalleryTime);
        Show("SingleGalleyTime", result.SingleGalleyTime);
        Show("PowerOnChannelId", result.PowerOnChannelId);
        Show("GalleryShowTime", result.GalleryShowTime);
        Show("CurrentClockId", result.CurrentClockId);
        Show("DateFormat", result.DateFormat);
        Show("HourMode", result.Time24, static x => (HourMode)x == HourMode.Hour12 ? "12" : "24");
        Show("TemperatureMode", result.TemperatureMode, static x => (TemperatureMode)x);
        Show("GyrateAngle", result.GyrateAngle);
        Show("Mirror", result.Mirror, static x => x == 1 ? "enable" : "disable");
        Show("LightSwitch", result.LightSwitch, static x => x == 1 ? "on" : "off");

        void Show<T>(string name, T? value, Func<T, object>? converter = null)
        {
            if (value is not null)
            {
                Console.WriteLine(converter is not null ? $"{name}: {converter(value)}" : $"{name}: {value}");
            }
        }
    }
}

//--------------------------------------------------------------------------------
// area
//--------------------------------------------------------------------------------
[Command("area", "Set area")]
public sealed class AreaCommand : BaseHostCommand, ICommandHandler
{
    [Option<double>("--lon", "-n", Description = "Longitude", Required = true)]
    public double Lon { get; set; }

    [Option<double>("--lat", "-t", Description = "Latitude", Required = true)]
    public double Lat { get; set; }

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.ConfigLogAndLatAsync(Lon, Lat);
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// timezone
//--------------------------------------------------------------------------------
[Command("timezone", "Set timezone")]
public sealed class TimezoneCommand : BaseHostCommand, ICommandHandler
{
    [Option<string>("--zone", "-z", Description = "Timezone", Required = true)]
    public string Zone { get; set; } = default!;

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.ConfigTimeZoneAsync(Zone);
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// temperature
//--------------------------------------------------------------------------------
[Command("temperature", "Set temperature mode")]
public sealed class TemperatureCommand : BaseHostCommand, ICommandHandler
{
    [Option<string>("--mode", "-m", Description = "Mode", Required = true, Completions = ["c", "celsius", "f", "fahrenheit"])]
    public string Mode { get; set; } = default!;

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.ConfigTemperatureModeAsync(
            Mode switch
            {
                "f" or "fahrenheit" => TemperatureMode.Fahrenheit,
                _ => TemperatureMode.Celsius
            });
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// hour
//--------------------------------------------------------------------------------
[Command("hour", "Set hour mode")]
public sealed class HourCommand : BaseHostCommand, ICommandHandler
{
    [Option<string>("--mode", "-m", Description = "Mode", Required = true, Completions = ["12", "24"])]
    public string Mode { get; set; } = default!;

    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.ConfigHourModeAsync(
            Mode switch
            {
                "24" => HourMode.Hour24,
                _ => HourMode.Hour12
            });
        result.EnsureSuccessStatus();
    }
}

//--------------------------------------------------------------------------------
// reboot
//--------------------------------------------------------------------------------
[Command("reboot", "Reboot")]
public sealed class RebootCommand : BaseHostCommand, ICommandHandler
{
    public async ValueTask ExecuteAsync(CommandContext context)
    {
        using var client = new DivoomClient(Host);
        var result = await client.RebootAsync();
        result.EnsureSuccessStatus();
    }
}
