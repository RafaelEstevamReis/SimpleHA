﻿namespace SimpleHA.Tests;
using Simple.HAApi;

public class SampleAPI
{
    public static async Task Run()
    {
        // Put your token and url in a text file
        var lines = File.ReadAllLines("token.txt");

        string haUrl = lines.Where(o => o.StartsWith("http")).First();
        string haToken = lines.Where(o => o.StartsWith("ey")).First();

        var instance = new Instance(new Uri(haUrl), haToken);
        // instance.IgnoreCertificatErrors = true;

        Console.WriteLine($"IsOnline: {await instance.CheckRunningAsync()}");

        /* Get data from configuration */
        var cfgSource = instance.Get<Simple.HAApi.Sources.Configuration>();

        var config = await cfgSource.GetConfigurationAsync();
        var entries = await cfgSource.GetConfigurationEntriesAsync();
        var check = await cfgSource.CheckConfigAsync();
        //var result = await cfgSource.GetReloadEntryAsync(entries[0].EntryId);

        /* Get states from entities */
        var statesSource = instance.Get<Simple.HAApi.Sources.States>();

        var all = await statesSource.GetStatesAsync();
        var sun = await statesSource.GetStateAsync("sun.sun");

        /* Get information from Services */
        var srvSource = instance.Get<Simple.HAApi.Sources.Service>();
        var services = await srvSource.GetServicesAsync();

        /* Get a stream of events */
        var eventSource = instance.Get<Simple.HAApi.Sources.Events>();
        var e = await eventSource.GetEventsAsync();

        var canSource = new CancellationTokenSource();
        eventSource.OnNewEvent += (s, e) =>
        {
            Console.WriteLine($" {e}");
        };
        var t = eventSource.CollectEventsAsync(canSource.Token);

        Console.WriteLine("END");

    }
}
