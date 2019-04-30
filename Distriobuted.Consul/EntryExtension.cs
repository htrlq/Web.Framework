using Consul;
using Newtonsoft.Json;
using System.Text;

namespace Distriobuted.Consul
{
    internal static class EntryExtension
    {
        public static KVPair ToKVPair<TEntry>(this TEntry entry, string key)
            where TEntry : class, new()
        {
            var valueString = JsonConvert.SerializeObject(entry);
            var valueBytes = Encoding.UTF8.GetBytes(valueString);

            return new KVPair(key)
            {
                Value = valueBytes
            };
        }
    }
}
