using Simple.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace Simple.HAApi.Sources
{
    public class Events : SourceBase
    {
        private HttpClient httpClient;
        public Events(ClientInfo info)
               : base(info)
        {
            info.ConfigureHttpClient(c => httpClient = c);
        }

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

    }
}
