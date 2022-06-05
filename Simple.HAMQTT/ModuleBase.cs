using MQTTnet;
using MQTTnet.Client.Disconnecting;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.HAMQTT
{
    public abstract class ModuleBase
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
    }
}
