using Simple.API;
using System;
using System.Net.Http;
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

        public TimeSpan Timeout { get => client.Timeout; set => client.Timeout = value; }

        public bool IgnoreCertificatErrors { get; set; } = false;

        internal readonly ClientInfo client;

        public Instance(Uri uri, string token)
            : this(uri, token, null)
        { }
        public Instance(Uri uri, string token, HttpMessageHandler handler)
        {
            Uri = uri;
            Token = token;

            if (handler == null) handler = new HttpClientHandler();
            if (handler is HttpClientHandler hhdl) hhdl.ServerCertificateCustomValidationCallback ??= certificateValidation;
#if !NETSTANDARD
            if (handler is SocketsHttpHandler shdl)
            {
                if(shdl.SslOptions != null) shdl.SslOptions = new System.Net.Security.SslClientAuthenticationOptions
                {
                    RemoteCertificateValidationCallback = certificateValidation,
                };
            }
#endif

            client = new ClientInfo(Uri.ToString(), handler)
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
            client.SetAuthorizationBearer(Token);
        }
        private bool certificateValidation(object sender,
                                           System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                           System.Security.Cryptography.X509Certificates.X509Chain chain,
                                           System.Net.Security.SslPolicyErrors policy)
        {
            if (IgnoreCertificatErrors) return true;

            return policy == System.Net.Security.SslPolicyErrors.None;
        }
        public void ConfigureInternalClient(Action<ClientInfo> action)
        {
            action(client);
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
            if (!info.IsSuccessStatusCode) throw new Exceptions.ClientException($"Request to `/api/` Failed with code {info.StatusCode}", info);
        }
        public async Task<string> ErrorLogsAsync()
        {
            var info = await client.GetAsync<string>("/api/error_log");
            if (!info.IsSuccessStatusCode) throw new Exceptions.ClientException($"Request to `/API/error_log` Failed with code {info.StatusCode}", info);

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
