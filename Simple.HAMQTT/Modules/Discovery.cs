using MQTTnet;
using MQTTnet.Client.Disconnecting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.HAMQTT.Modules
{
    public class Discovery : ModuleBase
    {
        public static string DefaultDiscoveryPrefix { get; set; } = "homeassistant";

        internal Discovery(BrokerInfo brokerInfo)
            : base(brokerInfo)
        { }

        public async Task RegisterAsync(string component, string nodeId, IEnumerable<Models.DeviceRegistry> entries)
        {

            var mqttFactory = new MqttFactory();

            using var mqttClient = mqttFactory.CreateMqttClient();
            var response = await mqttClient.ConnectAsync(brokerInfo.MqttClientOptions, CancellationToken.None);

            string nodeIdPath = "";
            if (nodeId != null) nodeIdPath = $"{nodeId}/";

            foreach (var registry in entries)
            {
                var applicationMessage = new MqttApplicationMessageBuilder()
                   .WithTopic($"{DefaultDiscoveryPrefix}/{component}/{nodeIdPath}{registry.DeviceId}/config")
                   .WithPayload(toJson(registry))
                   .Build();

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
            }

            await mqttClient.DisconnectAsync(new MqttClientDisconnectOptions(), CancellationToken.None);
        }


    }
}
