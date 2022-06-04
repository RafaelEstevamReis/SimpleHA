using MQTTnet;
using MQTTnet.Client.Disconnecting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.HAMQTT.Modules
{
    public class Discovery : ModuleBase
    {
        public static string DefaultDiscoveryPrefix { get; set; } = "homeassistant";

        public Discovery(BrokerInfo brokerInfo)
            : base(brokerInfo)
        { }

        public async Task RegisterAsync(string nodeId, IEnumerable<Models.DeviceRegistry> entries)
        {
            var mqttFactory = new MqttFactory();

            using var mqttClient = mqttFactory.CreateMqttClient();
            var response = await mqttClient.ConnectAsync(brokerInfo.MqttClientOptions, CancellationToken.None);

            string nodeIdPath = "";
            if (nodeId != null) nodeIdPath = $"{nodeId}/";

            foreach (var registry in entries)
            {
                var applicationMessage = new MqttApplicationMessageBuilder()
                   .WithTopic($"{DefaultDiscoveryPrefix}/{registry.Component}/{nodeIdPath}{registry.DeviceId}/config")
                   .WithPayload(toJson(registry))
                   .Build();

                var result = await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
                result = result;
            }

            await mqttClient.DisconnectAsync(new MqttClientDisconnectOptions(), CancellationToken.None);
        }


    }
}
