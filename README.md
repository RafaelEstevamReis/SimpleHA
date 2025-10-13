# Simple.HA
A simple C# Home-Assistant library

|  Packages      |                          |
| -------------- | ------------------------ |
| Simple.HAApi:  | [![NuGet](https://img.shields.io/nuget/v/Simple.HAApi)](https://www.nuget.org/packages/Simple.HAApi/)   |
| Simple.HAMQTT: | [![NuGet](https://img.shields.io/nuget/v/Simple.HAMQTT)](https://www.nuget.org/packages/Simple.HAMQTT/) |


## What is?

This is a C# library to interact with [HomeAssitant](https://home-assistant.io)

Home-Assitant has a developer docs webpage, but with almost nothing in it. \
There are only some endpoints (eg the `api/stream` is not documented) with no model specification

Some (almost none) source documentation: https://developers.home-assistant.io/docs/api/rest/

## How do I use it?

1. Create an Instance
~~~C#
var instance = new Instance(new Uri("https://myHA.io:8123"), yourToken);
// You can ignore SSL errors if needed:
instance.IgnoreCertificatErrors = true;
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
// Or call a service for a single entity
await srvSource.CallService(Service.Light.TurnOn, "switch.office");
// or multiple
await srvSource.CallService(Service.Light.TurnOn, "switch.office1", "switch.office2", ..., "switch.officeN");
~~~

6. Get a continuous stream of events
~~~C#
// Get a source
var eventSource = instance.Get<Simple.HAApi.Sources.Events>();
// Get a cancellation token to stop the process anytime
var canSource = new CancellationTokenSource();
eventSource.OnNewEvent += (s, e) =>
{
    Console.WriteLine($" {e}");
};
var t = eventSource.CollectEventsAsync(canSource.Token);
~~~

