using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Misc.CacheLookup.Areas.Admin.Factories;
using Nop.Plugin.Misc.CacheLookup.Areas.Admin.Services;

namespace Nop.Plugin.Misc.CacheLookup.Areas.Infrastructure;
public class NopStartup : INopStartup
{
    public int Order => 3000;

    public void Configure(IApplicationBuilder application)
    {
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICacheLookupService, CacheLookupService>();
        services.AddTransient<ICacheItemModelFactory, CacheItemModelFactory>();
    }
}
