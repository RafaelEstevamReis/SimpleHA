using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Threading.Tasks;

namespace Simple.HAMQTT
{
    internal class Broker
    {
        //internal BrokerInfo brokerInfo;

        public string Address { get; private set; }
        public int Port { get; private set; } // = 1883;

        public string UserName { get; private set; }

        private MqttClientOptions options;

        public Broker(string address, int port = 1883)
            : this(address, port, null, null) { }
        public Broker(string address, int port, string userName, string password)
        {
            Address = address;
            Port = port;
            UserName = userName;

            var builder = new MqttClientOptionsBuilder()
                                        .WithTcpServer(address, port);
            if (userName != null)
            {
                builder = builder.WithCredentials(UserName, password);
            }

            options = builder.Build();
        }

        internal BrokerInfo getNewBrokerInfo()
            => new BrokerInfo(options);


    }
    public class BrokerInfo : IDisposable
    {
        private MqttClientOptions options;
        private MqttFactory mqttFactory;
        private IManagedMqttClient managedMqttClient;

        internal BrokerInfo(MqttClientOptions options)
        {
            this.options = options;
            mqttFactory = new MqttFactory();
            managedMqttClient = mqttFactory.CreateManagedMqttClient();
        }

        public IManagedMqttClient GetClient()
            => managedMqttClient;

        public void Dispose()
        {
            managedMqttClient?.Dispose();
        }
    }
    internal static class BrokerExtension
    {
        public static TSource Get<TSource>(this Broker broker)
            where TSource : ModuleBase
        {
            var bi = broker.getNewBrokerInfo();
            return (TSource)Activator.CreateInstance(typeof(TSource), bi);
        }
    }
}
