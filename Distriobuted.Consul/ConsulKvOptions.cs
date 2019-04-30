using Microsoft.Extensions.Options;

namespace Distriobuted.Consul
{
    internal class ConsulKvOptions<TOptions> : IOptions<TOptions>
        where TOptions : class, new()
    {
        public TOptions Value { get; }

        public ConsulKvOptions(TOptions value)
        {
            Value = value;
        }
    }

}
