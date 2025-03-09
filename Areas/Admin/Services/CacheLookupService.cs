using System.Collections;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Plugin.Misc.CacheLookup.Areas.Admin.Extensions;
using Nop.Plugin.Misc.CacheLookup.Areas.Admin.Models;

namespace Nop.Plugin.Misc.CacheLookup.Areas.Admin.Services;
public class CacheLookupService : ICacheLookupService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IList<CacheItemModel> _cacheItems = new List<CacheItemModel>();

    public CacheLookupService(IMemoryCache memoryCache) => _memoryCache = memoryCache;

    public void RefreshCacheItems() =>
        _cacheItems.Clear();
    public void DeleteCacheItem(string key)
    {
        var cacheItem = _cacheItems.FirstOrDefault(it => it.Key.ToLower() == key.ToLower());
        if (cacheItem != null)
        {
            _memoryCache.Remove(key);
            _cacheItems.Remove(cacheItem);
        }

    }

    public IPagedList<CacheItemModel> GetCacheItems(int page, int pageSize, string searchName = "")
    {
        var searchNameValid = !string.IsNullOrEmpty(searchName);

        if (_cacheItems.Any() && !searchNameValid)
            return _cacheItems.ToPagedList(page, pageSize);

        if (_cacheItems.Any() && searchNameValid)
            return GetFilteredCacheItems(page, pageSize, searchName);

        return FetchAllCacheItems(page, pageSize);
    }

    private IPagedList<CacheItemModel> FetchAllCacheItems(int page, int pageSize, string searchName = "")
    {
        var coherentStateField = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);
        var coherentState = coherentStateField?.GetValue(_memoryCache);

        if (coherentState is not null)
        {
            var stringEntriesField = coherentState.GetType().GetField("_stringEntries", BindingFlags.NonPublic | BindingFlags.Instance);
            var stringEntries = stringEntriesField?.GetValue(coherentState) as ICollection;

            if (stringEntries != null)
                foreach (var entry in stringEntries)
                    CacheEntryProcessing(entry, searchName);

            var nonStringEntriesField = coherentState.GetType().GetField("_nonStringEntries", BindingFlags.NonPublic | BindingFlags.Instance);
            var nonStringEntries = nonStringEntriesField?.GetValue(coherentState) as ICollection;

            if (nonStringEntries != null)
                foreach (var entry in nonStringEntries)
                    CacheEntryProcessing(entry, searchName);
        }

        return _cacheItems.ToPagedList(page, pageSize);
    }

    private IPagedList<CacheItemModel> GetFilteredCacheItems(int page, int pageSize, string searchName)
    {
        IList<CacheItemModel> filteredCacheItems = new List<CacheItemModel>();

        foreach (var item in _cacheItems)
            if (item.Key.ToString().Contains(searchName, StringComparison.OrdinalIgnoreCase))
                filteredCacheItems.Add(item);

        return filteredCacheItems.ToPagedList(page, pageSize);
    }

    private void CacheItemProcessing(object key, ICacheEntry entryValue)
    {
        var cachedItem = new CacheItemModel();
        cachedItem.Key = key.ToString();

        var value = entryValue.Value;
        // let's identify the type of the value 
        if (value is IDictionary)
        {
            cachedItem.Type = "Dictionary";
            cachedItem.Count = ((IDictionary)value).Count;

        }
        else if (value is IList)
        {
            cachedItem.Type = "List";
            cachedItem.Count = ((IList)value).Count;

        }
        else
            cachedItem.Type = value.GetType().Name;

        try
        {
            // let's assign the value 
            cachedItem.Value = JsonConvert.SerializeObject(value, Formatting.Indented);

        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message.ToString());
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }


        // let's move to the size

        var size = 0;
        if (value is IEnumerable)
            foreach (var item in (IEnumerable)value)
                size += GetObjectSize(item);
        else
            size = GetObjectSize(value);

        cachedItem.Size = size;

        // expiration date 
        var utcAbsExpProp = entryValue.AbsoluteExpiration;
        var entryExpiry = utcAbsExpProp;

        if (entryExpiry != null)
        {
            var tz = TimeZoneInfo.Local;
            var date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(entryExpiry.Value.UtcDateTime,
                tz.Id);
            cachedItem.ExpiryDate = date;
        }

        if (cachedItem.Key != null && cachedItem.Value != null)
            _cacheItems.Add(cachedItem);
    }

    private void CacheEntryProcessing(object? entry, string? searchName)
    {
        var entryFieldKey = entry?.GetType().GetProperty("Key");
        var entryFieldValue = entry?.GetType().GetProperty("Value");
        var key = entryFieldKey?.GetValue(entry);

        var value = (ICacheEntry)entryFieldValue?.GetValue(entry);
        CacheItemProcessing(key, value);
    }

    private static int GetObjectSize(object obj)
    {
        return (int)typeof(BareMetal)
                            .GetMethod("SizeOf")
                            .MakeGenericMethod(obj.GetType())
                            .Invoke(obj, null);
    }
    
}
