using MQTTnet;
using MQTTnet.Client.Disconnecting;
using System;
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
            if (nodeId is null) throw new ArgumentNullException(nameof(nodeId));

            var mqttFactory = new MqttFactory();

            using var mqttClient = mqttFactory.CreateMqttClient();
            var response = await mqttClient.ConnectAsync(brokerInfo.MqttClientOptions, CancellationToken.None);

            //string nodeIdPath = "";
            //if (nodeId != null) nodeIdPath = $"{nodeId}/";

            foreach (var registry in entries)
            {
                var applicationMessage = new MqttApplicationMessageBuilder()
                   .WithTopic($"{DefaultDiscoveryPrefix}/{registry.Component}/{nodeId}/{registry.DeviceId}/config")
                   .WithPayload(toJson(registry))
                   //.WithRetainFlag() // Retain: The -r switch is added to retain the configuration topic in the broker.
                   //                  // Without this, the sensor will not be available after Home Assistant restarts.
                   .Build();

                Console.WriteLine(applicationMessage.Topic);
                Console.WriteLine(">" + System.Text.Encoding.UTF8.GetString(applicationMessage.Payload));

                var result = await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
                result = result;
            }

            await DisconnectAsync(mqttClient);
        }

    }
}
