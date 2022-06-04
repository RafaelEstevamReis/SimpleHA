namespace SimpleHA.Tests;

using MQTTnet;
using Simple.HAMQTT;
using Simple.HAMQTT.Models;
using Simple.HAMQTT.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SampleMqtt
{
    public static async Task Run()
    {
        // Put your server in a text file
        var lines = File.ReadAllLines("mqtt.txt");

        string basicTopic = "ha/sample";
        string availTopic = $"{basicTopic}/status";
        string batTopic = $"{basicTopic}/sensor/battery_test";
        string cpuTopic = $"{basicTopic}/sensor/cpu_test";

        Broker broker = new Broker(lines[0]);
        var publisher = broker.Get<Publish>();
        //var subcriber = broker.Get<Subscribe>();
        //await subcriber.SubscribeTo(availTopic, processStatusMessageAsync);

        var discovery = broker.Get<Discovery>();
        var devices = new DeviceRegistry[]
        {
            SensorRegistry.Build("Test Battery Level", batTopic , "battery", "%", "ha_battery", availabilityTopic: availTopic),
            SensorRegistry.Build("Test CPU", cpuTopic, null, "%", "cpu_use", availabilityTopic: availTopic),
        };
        await discovery.RegisterAsync("SimpleHA", devices);

        await publisher.PublishString(availTopic, "online");
        await publisher.PublishNumber(batTopic, 57.2);
        await publisher.PublishNumber(cpuTopic, 0.1);

        publisher = publisher;

        while (true)
        {
            for (int i = 0; i < 30; i++) // 30s
            {
                await Task.Delay(1000);
            }
            await publisher.PublishString(availTopic, "online");
            Console.WriteLine("ONLINE");
        }
    }

    private static async Task processStatusMessageAsync(Subscribe sub, MqttApplicationMessageReceivedEventArgs arg)
    {
        var msg = sub.As<string>(arg.ApplicationMessage);

        await Task.CompletedTask;
    }
}
