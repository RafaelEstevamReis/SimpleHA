using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Simple.HAMQTT.Modules
{
    public class Subscribe : ModuleBase
    {
        public Subscribe(BrokerInfo brokerInfo)
            : base(brokerInfo)
        { }

        public async Task SubscribeTo(string topic, Func<Subscribe, MqttApplicationMessageReceivedEventArgs, Task> callback)
        {
            var mqttClient = brokerInfo.GetClient();

            var filter = new MqttTopicFilterBuilder()
                .WithTopic(topic)
                .Build();

            await mqttClient.SubscribeAsync(new MqttTopicFilter[] { filter });
            mqttClient.ApplicationMessageReceivedAsync += async (e) =>
            {
                await callback(this, e);
            };
        }

        public T As<T>(MqttApplicationMessage message)
        {
            var json = Encoding.UTF8.GetString(message.Payload);

            if (typeof(T) == typeof(string)) return (T)(object)json;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }
}
