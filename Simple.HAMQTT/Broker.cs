using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.HAMQTT
{
    public class Broker
    {
        internal BrokerInfo brokerInfo;

        public string Address { get; private set; }
        public int Port { get; private set; } // = 1883;

        public string UserName { get; private set; }

        public Broker(string address, int port = 1883)
            : this(address, port, null, null) { }
        public Broker(string address, int port, string userName, string password)
        {
            Address = address;
            Port = port;
            UserName = userName;

            var optBuilder = new MqttClientOptionsBuilder()
                                        .WithTcpServer(address, port);
            if (userName != null)
            {
                optBuilder = optBuilder.WithCredentials(UserName, password);
            }

            brokerInfo = new BrokerInfo()
            {
                MqttClientOptions = optBuilder.Build(),
            };

        }

    }
    public class BrokerInfo
    {
        private static object lockObj = new();

        private MqttFactory mqttFactory;
        private IMqttClient mqttClient;

        internal IMqttClientOptions MqttClientOptions { get; set; }


        internal MqttFactory GetFactory()
        {
            if (mqttFactory == null) mqttFactory = new MqttFactory();
            return mqttFactory;
        }
        internal IMqttClient GetClient()
        {
            if (mqttClient == null)
            {
                mqttClient = GetFactory().CreateMqttClient();
            }

            return mqttClient;
        }
        internal async Task<IMqttClient> GetConnectedClientAsync()
        {
            GetClient();

            if (!mqttClient.IsConnected)
            {
                await Task.Run(() =>
                {
                    lock (lockObj)
                    {
                        var response = mqttClient.ConnectAsync(MqttClientOptions, CancellationToken.None).Result;
                        response = response;
                    }
                });
            }

            return mqttClient;
        }
    }
    public static class BrokerExtension
    {
        public static TSource Get<TSource>(this Broker broker)
            where TSource : ModuleBase
        {
            return (TSource)Activator.CreateInstance(typeof(TSource), broker.brokerInfo);
        }
    }
}
