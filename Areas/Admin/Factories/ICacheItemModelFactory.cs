using Nop.Plugin.Misc.CacheLookup.Areas.Admin.Models;

namespace Nop.Plugin.Misc.CacheLookup.Areas.Admin.Factories;
public interface ICacheItemModelFactory
{
    public CacheItemListDetails PrepareCacheItemModelList(CacheItemSearchModel searchModel);
}
