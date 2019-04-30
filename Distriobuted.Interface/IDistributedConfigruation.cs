using System;
using System.Threading.Tasks;

namespace Distriobuted.Interface
{
    public interface IDistributedConfigruation
    {
        Task<bool> TryImportAsync<TConfiguration>(string key, TConfiguration value)
            where TConfiguration : class, new();

        Task<bool> TryDeleteAsync(string key);
    }
}
