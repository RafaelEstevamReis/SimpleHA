using Simple.API;
using System;
using System.Threading.Tasks;

namespace Simple.HAApi
{
    public class Instance
    {
        // Docs:
        // https://developers.home-assistant.io/docs/api/rest/

        public Uri Uri { get; private set; }
        public string Token { get; private set; }

        internal readonly ClientInfo client;

        public Instance(Uri uri, string token)
        {
            Uri = uri;
            Token = token;

            client = new ClientInfo(Uri.ToString());
            client.SetAuthorizationBearer(Token);
        }

        public async Task IsRunningAsync()
        {
            var info = await client.GetAsync<string>("/api/");
            if (!info.IsSuccessStatusCode) throw new Exceptions.ClientExeption($"Request to `/api/` Failed with code {info.StatusCode}", info);
        }
        public async Task<string> ErrorLogsAsync()
        {
            var info = await client.GetAsync<string>("/api/error_log");
            if (!info.IsSuccessStatusCode) throw new Exceptions.ClientExeption($"Request to `/API/error_log` Failed with code {info.StatusCode}", info);

            return info.Data;
        }
    }
    public static class InstanceExtension
    {
        public static TSource Get<TSource>(this Instance instance)
            where TSource : SourceBase
        {
            return (TSource)Activator.CreateInstance(typeof(TSource), instance.client);
        }
    }
}
