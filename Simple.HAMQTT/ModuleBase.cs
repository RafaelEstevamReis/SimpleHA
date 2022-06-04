namespace Simple.HAMQTT
{
    public abstract class ModuleBase
    {
        protected BrokerInfo brokerInfo;

        protected ModuleBase(BrokerInfo brokerInfo)
        {
            this.brokerInfo = brokerInfo;
        }

        protected string toJson(object registry)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(registry);
        }
    }
}
