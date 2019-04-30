using Consul;
using Distriobuted.Interface;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Distriobuted.Consul
{
    internal class ConsulDistributedConfigruation : IDistributedConfigruation
    {
        private ConsulClient Consul { get; }

        public ConsulDistributedConfigruation(IConfiguration<ConsulClientConfiguration> configuration)
        {
            Consul = new ConsulClient((_config)=>
            {
                var value = configuration.Value;

                _config.Address = value.Address;
                _config.Datacenter = value.Datacenter;
                _config.Token = value.Token;
                _config.WaitTime = value.WaitTime;
            });
        }

        public async Task<bool> TryImportAsync<TConfiguration>(string key, TConfiguration value)
            where TConfiguration:class,new()
        {
            var response = await Consul.KV.Put(value.ToKVPair(key));

            return response.Response;
        }

        public async Task<bool> TryDeleteAsync(string key)
        {
            var response = await Consul.KV.Delete(key);

            return response.Response;
        }

        public async Task<TEntity> TryGetAsync<TEntity>(string key)
            where TEntity:class,new()
        {
            var query = await Consul.KV.Get(key);

            if (query.StatusCode == HttpStatusCode.OK)
            {
                var response = query.Response;
                var valueBytes = response.Value;
                var jsonString = Encoding.UTF8.GetString(valueBytes);

                return JsonConvert.DeserializeObject<TEntity>(jsonString);
            }

            throw new Exception($"Key:{key} StatusCode:{query.StatusCode}");
        }
    }

}
