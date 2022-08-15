using System;

namespace Simple.HAMQTT
{
    public abstract class ModuleBase : IDisposable
    {
        protected BrokerInfo brokerInfo;

        protected ModuleBase(BrokerInfo brokerInfo)
        {
            this.brokerInfo = brokerInfo;
        }
        protected string toJson(object obj)
        {
            if (obj is string) return (string)obj;
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public void Dispose()
        {
            brokerInfo?.Dispose();
        }

    }
}
