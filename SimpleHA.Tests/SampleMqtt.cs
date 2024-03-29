﻿namespace SimpleHA.Tests;

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using Simple.HAMQTT;
using Simple.HAMQTT.Models;
using System;
using System.Threading.Tasks;

public class SampleMqtt
{
    public static async Task Run()
    {
        var random = new Random();
        // Put your server in a text file
        var server = File.ReadAllLines("mqtt.txt")[0];

        string basicTopic = "ha/sample";
        string availTopic = $"{basicTopic}/status";
        string batTopic = $"{basicTopic}/sensor/battery_test";
        string cpuTopic = $"{basicTopic}/sensor/cpu_test";
        string btnTopic = $"{basicTopic}/button/btn_test";

        var mqttFactory = new MqttFactory();

        using (var managedMqttClient = mqttFactory.CreateManagedMqttClient())
        {
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(server, 1883)
                .Build();

            var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
                .WithClientOptions(mqttClientOptions)
                .Build();

            await managedMqttClient.StartAsync(managedMqttClientOptions);


            var devices = new DeviceRegistry[]
            {
                SensorRegistry.Build("Test Battery Level", batTopic, SensorRegistry.CLASS_BATTERY, "%", "ha_battery", availabilityTopic: availTopic),
                SensorRegistry.Build("Test CPU", cpuTopic, null, "%", "cpu_use", availabilityTopic: availTopic),
                ButtonRegistry.Build("Test Button", btnTopic, null, "btnTest"),
            };
            await managedMqttClient.RegisterAsync("SimpleHA", devices);

            float pBat = random.Next(40, 70);
            while (true)
            {
                await managedMqttClient.PublishString(availTopic, "online");
                Console.WriteLine("ONLINE");

                if (DateTime.Now.Hour % 2 == 0) pBat -= 0.3f;
                else pBat += 0.2f;

                await managedMqttClient.PublishNumber(batTopic, Math.Round(pBat, 1));
                await managedMqttClient.PublishNumber(cpuTopic, Math.Round(random.NextDouble() / 3, 2));

                for (int i = 0; i < 60; i++) // 30s
                {
                    await Task.Delay(1000);
                }
            }
            // Final flush queue until empty, or until 1000ms
            //SpinWait.SpinUntil(() => managedMqttClient.PendingApplicationMessagesCount == 0, 1000);

        }
    }


    //private static async Task processStatusMessageAsync(Subscribe sub, MqttApplicationMessageReceivedEventArgs arg)
    //{
    //    var msg = sub.As<string>(arg.ApplicationMessage);
    //    Console.WriteLine(msg);
    //    await Task.CompletedTask;
    //}
}
