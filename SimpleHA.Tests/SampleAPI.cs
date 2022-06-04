namespace SimpleHA.Tests;
using Simple.HAApi;

public class SampleAPI
{
    public static async void Run()
    {
        // Put your token and url in a text file
        var lines = File.ReadAllLines("token.txt");

        string haUrl = lines.Where(o => o.StartsWith("http")).First();
        string haToken = lines.Where(o => o.StartsWith("ey")).First();

        var instance = new Instance(new Uri(haUrl), haToken);


        Console.WriteLine($"IsOnline: { await instance.CheckRunningAsync() }");

        /* Get data from configuration */
        var cfgSource = instance.Get<Simple.HAApi.Sources.Configuration>();

        var config = await cfgSource.GetConfigurationAsync();
        var entries = await cfgSource.GetConfigurationEntriesAsync();


        /* Get states from entities */
        var statesSource = instance.Get<Simple.HAApi.Sources.States>();

        var all = await statesSource.GetStatesAsync();
        var sun = await statesSource.GetStateAsync("sun.sun");

        /* Get information from Services */
        var srvSource = instance.Get<Simple.HAApi.Sources.Service>();
        var services = await srvSource.GetServicesAsync();

        /* Get a stream of events */
        var eventSource = instance.Get<Simple.HAApi.Sources.Events>();

        var canSource = new CancellationTokenSource();
        var stream = eventSource.GetEvents(canSource.Token);

        Console.WriteLine("Continuous stream of events (press enter to stop)");
        foreach (var e in stream)
        {
            Console.WriteLine($" {e}");
            if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Enter)
            {
                Console.WriteLine("CANCEL");
                canSource.Cancel();
            }
        }

        Console.WriteLine("END");

    }
}
