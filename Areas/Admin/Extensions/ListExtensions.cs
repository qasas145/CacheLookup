using Nop.Core;

namespace Nop.Plugin.Misc.CacheLookup.Areas.Admin.Extensions;
public static class ListExtensions
{
    public static IPagedList<T> ToPagedList<T>(this IList<T> list, int page, int pageSize)
    {
        var totalCount = list.Count;
        var newList = list.Skip(page * pageSize).Take(pageSize).ToList();
        return new PagedList<T>(newList, page, pageSize, totalCount);
    }
}
