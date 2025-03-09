using Nop.Services.Plugins;

namespace Nop.Plugin.Misc.CacheLookup;
public class CacheLookupPlugin : BasePlugin
{
    public override string GetConfigurationPageUrl()
    {
        return "/CacheLookup/Configure";
    }
    public override Task InstallAsync()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("This is the installation method");
        Console.ForegroundColor = ConsoleColor.White;
        return base.InstallAsync();
    }
}
