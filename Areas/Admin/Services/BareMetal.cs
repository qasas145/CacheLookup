using System.Runtime.CompilerServices;

namespace Nop.Plugin.Misc.CacheLookup.Areas.Admin.Services;

public static class BareMetal
{
    public static unsafe int SizeOf<T>()
    {
        return Unsafe.SizeOf<T>();
    }
}