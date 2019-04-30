using Consul;
using Distriobuted.Interface;

namespace Distriobuted.Consul
{
    internal class ConsulConfiguration: IConfiguration<ConsulClientConfiguration>
    {
        private ConsulClientConfiguration Configuration { get; set; }
        public void SetConfiguration(ConsulClientConfiguration configuration)
        {
            Configuration = configuration;
        }

        public ConsulClientConfiguration Value => Configuration;
    }
}
