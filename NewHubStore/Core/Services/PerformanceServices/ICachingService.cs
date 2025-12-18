using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services.PerformanceServices
{
    public interface ICachingService
    {
        Task CacheItemUnitsAsync(int itemId);
        Task CacheOpeningBalancesAsync(int fiscalYear);
        Task InvalidateCacheAsync(string key);
        Task WarmupCacheAsync();
    }
}
