using Simple.API;
using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Simple.HAApi
{
    public class Instance
    {
        // Docs:
        // https://developers.home-assistant.io/docs/api/rest/
        // https://github.com/home-assistant/core/blob/dev/homeassistant/components/api/__init__.py

        public Uri Uri { get; private set; }
        public string Token { get; private set; }

        public bool IgnoreCertificatErrors { get; set; } = false;

        internal readonly ClientInfo client;

        public Instance(Uri uri, string token)
        {
            Uri = uri;
            Token = token;

            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = certificateValidation,
            };
            client = new ClientInfo(Uri.ToString(), handler);
            client.SetAuthorizationBearer(Token);
        }
        private bool certificateValidation(HttpRequestMessage message, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors policy)
        {
            if (IgnoreCertificatErrors) return true;

            return false;
        }

        public async Task<bool> CheckRunningAsync()
        {
            try
            {
                await IsRunningAsync();
                return true;
            }
            catch { return false; }
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
