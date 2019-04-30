using Consul;
using Distriobuted.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Distriobuted.Consul
{
    internal class ConsulKvCache: IKvCache, IDisposable
    {
        private volatile object _locked = new object();

        private ConsulClient Consul { get; }
        private List<KVPair> KvPairs { get; set; } = new List<KVPair>();
        private Task RefreshCacheTask { get; set; }
        private CancellationTokenSource Source { get; }

        public ConsulKvCache(IConfiguration<ConsulClientConfiguration> configuration)
        {
            Consul = new ConsulClient((_config) =>
            {
                var value = configuration.Value;

                _config.Address = value.Address;
                _config.Datacenter = value.Datacenter;
                _config.Token = value.Token;
                _config.WaitTime = value.WaitTime;
            });

            Source = new CancellationTokenSource();
            RefreshCacheTask = Task.Factory.StartNew(async () =>
            {
                while (!Source.Token.IsCancellationRequested)
                {
                    try
                    {

                        var query = await Consul.KV.List("", Source.Token);

                        if (query.StatusCode == HttpStatusCode.OK)
                        {
                            lock(_locked)
                                KvPairs = query.Response.ToList();
                        }
                        else
                        {
                            lock (_locked)
                                KvPairs = new List<KVPair>();
                        }

                        await Task.Delay(1500);
                    }
                    catch(Exception ex)
                    {

                    }
                }

            }, Source.Token);
        }

        public TEntity Get<TEntity>(string key)
            where TEntity:class,new()
        {
            lock (_locked)
            {
                if (KvPairs.Any(_pair => _pair.Key == key))
                {
                    var first = KvPairs.FirstOrDefault(_pair => _pair.Key == key);
                    var valueBytes = first?.Value;
                    var jsonString = Encoding.UTF8.GetString(valueBytes);

                    var entity = JsonConvert.DeserializeObject(jsonString, typeof(TEntity));

                    return entity as TEntity;
                }
            }

            return null;
        }
        public void StartCache()
        {
            var query = Consul.KV.List("", Source.Token).Result;

            if (query.StatusCode == HttpStatusCode.OK)
            {
                lock (_locked)
                    KvPairs = query.Response.ToList();
            }
            else
            {
                lock (_locked)
                    KvPairs = new List<KVPair>();
            }
        }

        public void Dispose()
        {
            Consul.Dispose();
            Source.Dispose();
            RefreshCacheTask.Dispose();
        }
    }

}
