using Divoom.Tools;

using Smart.CommandLine.Hosting;

var builder = CommandHost.CreateBuilder(args);
builder.ConfigureCommands(commands =>
{
    commands.ConfigureRootCommand(root =>
    {
        root.WithDescription("Divoom client");
    });

    commands.AddCommands();
});

var host = builder.Build();
return await host.RunAsync();
