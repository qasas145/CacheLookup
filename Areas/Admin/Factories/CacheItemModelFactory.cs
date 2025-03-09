using Nop.Plugin.Misc.CacheLookup.Areas.Admin.Models;
using Nop.Plugin.Misc.CacheLookup.Areas.Admin.Services;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.CacheLookup.Areas.Admin.Factories;
public class CacheItemModelFactory : ICacheItemModelFactory
{
    private readonly ICacheLookupService _cacheLookupService;
    public CacheItemModelFactory(ICacheLookupService cacheLookupService)
    {
        _cacheLookupService = cacheLookupService;
    }
    public CacheItemListDetails PrepareCacheItemModelList(CacheItemSearchModel searchModel)
    {
        var cachedItems = _cacheLookupService.GetCacheItems(searchModel.Page - 1, searchModel.PageSize, searchModel.ItemName); // add paging here 

        var model = new CacheItemListDetails().PrepareToGrid(searchModel, cachedItems, () =>
        {
            return cachedItems.Select(rate => rate);
        });
        return model;
    }
}
