using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.CacheLookup.Areas.Admin.Models;
public record CacheItemSearchModel : BaseSearchModel
{
    public string ItemName { get; set; }
}
