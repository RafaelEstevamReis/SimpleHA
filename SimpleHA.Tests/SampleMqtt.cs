namespace SimpleHA.Tests;

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

        Broker broker = new Broker(lines[0]);

        var discovery = broker.Get<Discovery>();

        var devices = new DeviceRegistry[]
        {
            SensorRegistry.Build("Battery Level", "ha/sensor/battery", "battery", "%", "battery"),
            SensorRegistry.Build("CPU", "ha/sensor/battery", null, "%", "cpu_use"),
        };

        await discovery.RegisterAsync("SimpleHA", devices);


    }
}
