using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CacheLookup.Areas.Admin.Models
{
    public record CacheItemModel : BaseNopModel
    {
        public string Key { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int? Count { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Size { get; set; }
    }

    public record CacheItemListDetails : BasePagedListModel<CacheItemModel>
    {


    }
}
