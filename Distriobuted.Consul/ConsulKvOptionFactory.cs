using Distriobuted.Interface;
using Microsoft.Extensions.Options;

namespace Distriobuted.Consul
{
    internal class ConsulKvOptionFactory<TOptions> : IOptionsFactory<TOptions>
        where TOptions : class, new()
    {
        private IKvCache Cache { get; }

        public ConsulKvOptionFactory(IKvCache cache)
        {
            Cache = cache;

            Cache.StartCache();
        }

        public TOptions Create(string name)
        {
            return Cache.Get<TOptions>(name);
        }
    }

}
