using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Nop.Plugin.Misc.CacheLookup.Areas.Admin.Factories;
using Nop.Plugin.Misc.CacheLookup.Areas.Admin.Models;
using Nop.Plugin.Misc.CacheLookup.Areas.Admin.Services;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.CacheLookup.Areas.Admin.Controllers;

[AuthorizeAdmin]
[Area("Admin")]
[AutoValidateAntiforgeryToken]
public class CacheLookupController : BasePluginController
{

    private readonly ICacheItemModelFactory _cacheItemModelFactory;
    private readonly IMemoryCache _memoryCache;
    private readonly ICacheLookupService _cacheLookupService;

    public CacheLookupController(ICacheItemModelFactory cacheItemModelFactory, 
        IMemoryCache memoryCache, 
        ICacheLookupService cacheLookupService)
    {
        _cacheItemModelFactory = cacheItemModelFactory;
        _memoryCache = memoryCache;
        _cacheLookupService = cacheLookupService;
    }

    public IActionResult CacheItems(CacheItemSearchModel searchModel)
    {
        var result = _cacheItemModelFactory.PrepareCacheItemModelList(searchModel);
        return Json(result);
    }
    public IActionResult DeleteCacheItem(string key)
    {
        _cacheLookupService.DeleteCacheItem(key);
        return RedirectToAction(nameof(CacheItems), new CacheItemSearchModel());
    }
    public IActionResult Configure()
    {
        _cacheLookupService.RefreshCacheItems();
        return ViewComponent("CacheLookup");
    }
}
