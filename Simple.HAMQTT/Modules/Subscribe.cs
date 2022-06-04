using MQTTnet;
using MQTTnet.Client;
using System;
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

        public async Task SubscribeTo(string topic, Func<Subscribe, MqttApplicationMessageReceivedEventArgs, Task> callback)
        {
            var mqttClient = brokerInfo.GetClient();
            mqttClient.UseApplicationMessageReceivedHandler(async e => await callback(this, e));
            //mqttClient.ApplicationMessageReceivedAsync += e =>
            //{
            //    Console.WriteLine("Received application message.");
            //    e.DumpToConsole();

            //    return Task.CompletedTask;
            //};

            await brokerInfo.GetConnectedClientAsync();

            var mqttSubscribeOptions = brokerInfo.GetFactory().CreateSubscribeOptionsBuilder()
               .WithTopicFilter(f => f.WithTopic(topic))
               .Build();
            var subResp = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

        }

        public T As<T>(MqttApplicationMessage message)
        {
            var json = Encoding.UTF8.GetString(message.Payload);

            if (typeof(T) == typeof(string)) return (T)(object)json;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }
}
