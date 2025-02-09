namespace SimpleHA.Tests;
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
        //var entries = await cfgSource.GetConfigurationEntriesAsync();
        var check = await cfgSource.CheckConfigAsync();
        //var result = await cfgSource.GetReloadEntryAsync(entries[0].EntryId);

        //var cam = instance.Get<Simple.HAApi.Sources.Camera>();
        //var img = await cam.GetImageAsync("camera.cam307pt");

        /* Get states from entities */
        var statesSource = instance.Get<Simple.HAApi.Sources.States>();
        var sun = await statesSource.GetStateAsync("sun.sun");
        var all = await statesSource.GetStatesAsync();
        var switches = all.OfDomain(Domains.SWITCH);
        var swIds = switches.GetIds();

        /* Get information from Services */
        var srvSource = instance.Get<Simple.HAApi.Sources.Service>();
        var services = await srvSource.GetServicesAsync();
        // turn light on
        var result = await srvSource.CallServiceAsync(Simple.HAApi.Sources.Service.Light.TurnOn, "bedroom.light");

        /* Get a stream of events */
        var eventSource = instance.Get<Simple.HAApi.Sources.Events>();
        var e = await eventSource.GetEventsAsync();

        var canSource = new CancellationTokenSource();
        eventSource.OnNewEvent += (s, e) =>
        {
            Console.WriteLine($" {e}");
        };
        var t = eventSource.CollectEventsAsync(canSource.Token);

        var hst = instance.Get<Simple.HAApi.Sources.History>();
        var allStateIds = all.Select(o => o.EntityId).Distinct().ToArray();
        var h = await hst.GetPeriodAsync(DateTime.Now.AddHours(-1), DateTime.Now, filterIds: allStateIds);
        var avg = h.BuildAverage().ToArray();

        Console.WriteLine("END");
    }
}
