using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Misc.CacheLookup.Areas.Admin.Models;

namespace Nop.Plugin.Misc.CacheLookup.Areas.Admin.Services;
public interface ICacheLookupService
{
    public IPagedList<CacheItemModel> GetCacheItems(int page, int pageSize, string searchName = "");
    public void DeleteCacheItem(string key);
    public void RefreshCacheItems();
}
