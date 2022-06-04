using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.HAMQTT.Modules
{
    public class Publish : ModuleBase
    {
        public Publish(BrokerInfo brokerInfo)
            : base(brokerInfo)
        { }

        //public async Task PublishTextValue(string topic, string text)
        //    => await publishObject(topic, new { value = text });
        //public async Task PublishString(string topic, string text)
        //    => await publishObject(topic, text);
        //public async Task PublishNumber(string topic, int value, string unit)
        //    => await publishObject(topic, new { value, unit });
        //public async Task PublishNumber(string topic, double value, string unit)
        //    => await publishObject(topic, new { value, unit });

        public async Task PublishString(string topic, string text)
            => await publishObject(topic, $"\"{text}\"", raw: true);
        public async Task PublishNumber(string topic, double value)
            => await publishObject(topic, $"{value}", raw: true);

        private async Task publishObject(string topic, object obj, bool raw = false)
        {

            var mqttFactory = new MqttFactory();

            using var mqttClient = mqttFactory.CreateMqttClient();
            var response = await mqttClient.ConnectAsync(brokerInfo.MqttClientOptions, CancellationToken.None);

            string content;

            if (raw)
            {
                content = (string)obj;
            }
            else
            {
                content = toJson(obj);
            }

            var applicationMessage = new MqttApplicationMessageBuilder()
               .WithTopic(topic)
               .WithPayload(content)
               .Build();

            var result = await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

            await DisconnectAsync(mqttClient);
        }
    }
}
