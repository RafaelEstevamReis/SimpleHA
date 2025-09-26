namespace Simple.HAApi.Sources;

using Simple.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

public class Events : SourceBase
{
    private HttpClient httpClient;
    public Events(ClientInfo info)
           : base(info)
    {
        info.ConfigureHttpClient(c => httpClient = c);
    }

    public event EventHandler<Models.EventModel> OnNewEvent;

    public async Task CollectEventsAsync(CancellationToken token)
    {
        if (OnNewEvent is null) throw new InvalidOperationException($"{nameof(OnNewEvent)} should not be null");

        var uri = new Uri(client.BaseUri, "api/stream");
        using var msg = new HttpRequestMessage(HttpMethod.Get, uri);
        using var response = await httpClient.SendAsync(msg, HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        var baseStream = await response.Content.ReadAsStreamAsync();
        // About cancelling ReadLineAsync:
        // https://stackoverflow.com/a/20132104/734639
        // There is no true cancellation in the underliyng system
        // so the cancellation must kill the connection,
        // forcing ReadLineAsync to return (or break)
        token.Register(() =>
        {
            // This will kill the stream and the connection
            // resulting in an exception during ReadLineAsync
            msg?.Dispose();
            response?.Dispose();
            baseStream?.Dispose();
        });
        try
        {
            using var sr = new StreamReader(baseStream);
            while (!sr.EndOfStream)
            {
                //if (token.IsCancellationRequested) break;
                var line = await sr.ReadLineAsync();

                if (line is null) break; // No more data on the stream
                if (string.IsNullOrEmpty(line)) continue;

                if (line.StartsWith("data: ")) line = line.Substring(6).Trim();
                if (line == "ping") continue; // keep-alive

                var evnt = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.EventModel>(line);

                OnNewEvent(this, evnt);
            }
        }
        catch
        {
            if (token.IsCancellationRequested) return;
            throw;
        }
    }


    [Obsolete("Use CollectEventsAsync instead")]
    public IEnumerable<Models.EventModel> GetEvents(CancellationToken token, params string[] entitiesIds)
    {
        var uri = new Uri(client.BaseUri, "api/stream");
        var msg = new HttpRequestMessage(HttpMethod.Get, uri);
        var response = httpClient.SendAsync(msg, HttpCompletionOption.ResponseHeadersRead).Result;

        response.EnsureSuccessStatusCode();

        using (var sr = new StreamReader(response.Content.ReadAsStreamAsync().Result))
        {
            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(1);

                var line = sr.ReadLine();
                if (string.IsNullOrEmpty(line)) continue;

                if (line.StartsWith("data: ")) line = line.Substring(6).Trim();
                if (line == "ping") continue; // keep-alive

                var evnt = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.EventModel>(line);

                if (entitiesIds != null && entitiesIds.Length > 0)
                {
                    if (!entitiesIds.Contains(evnt.EventData?.EntityId)) continue;
                }

                yield return evnt;
            }
        }

        response.Dispose();
        msg.Dispose();
    }

    public async Task<IEnumerable<Models.EventListenersModel>> GetEventsAsync()
        => await GetAsync< IEnumerable<Models.EventListenersModel>>("/api/events");

    public async Task<string> FireEventAsync(string eventType, object payload)
        => await PostAsync<string>($"/api/events/{eventType}", payload);
}
