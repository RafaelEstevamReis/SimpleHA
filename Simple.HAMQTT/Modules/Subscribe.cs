using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.HAMQTT.Modules
{
    public class Subscribe : ModuleBase
    {
        public Subscribe(BrokerInfo brokerInfo)
            : base(brokerInfo)
        { }

        public async Task<IMqttClient> SubscribeTo(string topic, Func<Subscribe, MqttApplicationMessageReceivedEventArgs, Task> callback)
        {
            var mqttFactory = new MqttFactory();

            var mqttClient = mqttFactory.CreateMqttClient();

            mqttClient.UseApplicationMessageReceivedHandler(async e => await callback(this, e));

            await mqttClient.ConnectAsync(brokerInfo.MqttClientOptions, CancellationToken.None);

            var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
               .WithTopicFilter(f => f.WithTopic(topic))
               .Build();
            await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

            return mqttClient;
        }

        public T As<T>(MqttApplicationMessage message)
        {
            var json = System.Text.Encoding.UTF8.GetString(message.Payload);

            if (typeof(T) == typeof(string)) return (T)(object)json;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }
}
