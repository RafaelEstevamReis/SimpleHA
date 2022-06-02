// Put your token and url in a text file
using Simple.HAApi;

var lines = File.ReadAllLines("token.txt");

string haUrl = lines.Where(o => o.StartsWith("http")).First();
string haToken = lines.Where(o => o.StartsWith("ey")).First();

var instance = new Instance(new Uri(haUrl), haToken);


Console.WriteLine($"IsOnline: { await instance.CheckRunningAsync() }" );

/* Get data from configuration */
var cfgSource = instance.Get<Simple.HAApi.Sources.Configuration>();

var config = await cfgSource.GetConfiguration();
var entries = await cfgSource.GetConfigurationEntries();


/* Get states from entities */
var statesSource = instance.Get<Simple.HAApi.Sources.States>();

var sun = await statesSource.GetState("sun.sun");

/* Get information from Services */
var srvSource = instance.Get<Simple.HAApi.Sources.Service>();
var services = await srvSource.GetServicesAsync();

/* Get a stream of events */
var eventSource = instance.Get<Simple.HAApi.Sources.Events>();

var canSource = new CancellationTokenSource();
var stream = eventSource.GetEvents(canSource.Token);


foreach(var e in stream)
{
    Console.WriteLine(e);
}

