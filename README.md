# Simple.HA
A simple c# Home-Assistant library


[![NuGet](https://buildstats.info/nuget/Simple.HAApi)](https://www.nuget.org/packages/Simple.HAApi/)

## What is?

This is a C# library to interact with [HomeAssitant](https://home-assistant.io)

Home-Assitant has a developer docs webpage, but with almost nothing in it. \
There are only some endpoints (eg the `api/stream` is not documented) with no model specification

Some (almost none) source documentation: https://developers.home-assistant.io/docs/api/rest/

## How do I use it?

1. Create an Instance
~~~C#
var instance = new Instance(new Uri("https://myHA.io:8123"), yourToken);
~~~

2. Basic diagnostics
~~~C#
bool isRunning = await instance.CheckRunningAsync();
string myErrors = await instance.ErrorLogsAsync();
~~~

3. Get configuration data
~~~C#
// Get a source
var cfgSource = instance.Get<Simple.HAApi.Sources.Configuration>();
// Get info as needed
var config = await cfgSource.GetConfigurationAsync();
var entries = await cfgSource.GetConfigurationEntriesAsync();
~~~

4. Get entity states
~~~C#
// Get a source
var statesSource = instance.Get<Simple.HAApi.Sources.States>();
// Get info as needed
var sun = await statesSource.GetStateAsync("sun.sun");
// Or get EVERYTHING
var all = await statesSource.GetStatesAsync();
~~~

5. Get information from and call Services
~~~C#
// Get a source
var srvSource = instance.Get<Simple.HAApi.Sources.Service>();
// Get all services
var services = await srvSource.GetServicesAsync();
// Or call a service
await srvSource.CallServiceForEntities("switch.turn_on", "switch.office");
~~~

6. Get a continuous stream of events
~~~C#
// Get a source
var eventSource = instance.Get<Simple.HAApi.Sources.Events>();
// Get a cancellation token to stop the process anytime
var canSource = new CancellationTokenSource();
// Get the enumerable
var stream = eventSource.GetEvents(canSource.Token);
// Show data
Console.WriteLine("Continuous stream of events");
foreach(var e in stream)
{
    Console.WriteLine($" {e}");
}
~~~

