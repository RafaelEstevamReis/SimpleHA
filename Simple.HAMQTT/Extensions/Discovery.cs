using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple.HAMQTT
{
    public static class Discovery
    {
        public static string DefaultDiscoveryPrefix { get; set; } = "homeassistant";

        public static async Task RegisterAsync(this IManagedMqttClient mqttClient, string nodeId, Models.DeviceRegistry entry, Models.DeviceInfo device = null)
            => await mqttClient.RegisterAsync(nodeId, new Models.DeviceRegistry[] { entry }, device);
        public static async Task RegisterAsync(this IManagedMqttClient mqttClient, string nodeId, IEnumerable<Models.DeviceRegistry> entries, Models.DeviceInfo device = null)
        {
            if (nodeId is null) throw new ArgumentNullException(nameof(nodeId));

            foreach (var registry in entries)
            {
                if (device != null && registry == null)
                {
                    registry.Device = device;
                }

                var applicationMessage = new MqttApplicationMessageBuilder()
                   .WithTopic($"{DefaultDiscoveryPrefix}/{registry.Component}/{nodeId}/{registry.DeviceId}/config")
                   .WithPayload(Helpers.ToJson(registry))
                   //.WithRetainFlag() // Retain: The -r switch is added to retain the configuration topic in the broker.
                   //                  // Without this, the sensor will not be available after Home Assistant restarts.
                   .Build();

                Console.WriteLine(applicationMessage.Topic);
                Console.WriteLine(">" + System.Text.Encoding.UTF8.GetString(applicationMessage.Payload));

                await mqttClient.EnqueueAsync(applicationMessage);
            }
        }

    }
}
