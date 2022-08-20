using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using System.Globalization;
using System.Threading.Tasks;

namespace Simple.HAMQTT
{
    public static class Publish
    {
        public static async Task PublishString(this IManagedMqttClient mqttClient, string topic, string text)
            => await publishObject(mqttClient, topic, $"{text}", raw: true);
        public static async Task PublishNumber(this IManagedMqttClient mqttClient, string topic, double value)
            => await publishObject(mqttClient, topic, value.ToString(CultureInfo.InvariantCulture), raw: true);

        private static async Task publishObject(IManagedMqttClient mqttClient, string topic, object obj, bool raw = false)
        {
            string content = raw ? (string)obj : Helpers.ToJson(obj);

            var applicationMessage = new MqttApplicationMessageBuilder()
               .WithTopic(topic)
               .WithPayload(content)
               .Build();

            await mqttClient.EnqueueAsync(applicationMessage);
        }
    }
}
