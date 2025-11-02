namespace Simple.HAApi;

using Simple.API;
using System;
using System.Net.Http;
using System.Threading.Tasks;

/// <summary>
/// Represents a HomeAssistant instance
/// </summary>
public class Instance
{
    // Docs:
    // https://developers.home-assistant.io/docs/api/rest/
    // https://github.com/home-assistant/core/blob/dev/homeassistant/components/api/__init__.py

    /// <summary>
    /// Current HomeAssistant URI
    /// </summary>
    public Uri Uri { get; private set; }
    /// <summary>
    /// Current used Token
    /// </summary>
    public string Token { get; private set; }
    /// <summary>
    /// Sets timeout for operations
    /// </summary>
    public TimeSpan Timeout { get => client.Timeout; set => client.Timeout = value; }
    /// <summary>
    /// Ignores any certificate errors, use with caution
    /// </summary>
    public bool IgnoreCertificatErrors { get; set; } = false;

    internal readonly ClientInfo client;


    /// <summary>
    /// Creates a new Instance to represent a HomeAssitant server
    /// </summary>
    /// <param name="uri">Server's URI to conenct to</param>
    /// <param name="token">Token to authenticate as</param>
    public Instance(Uri uri, string token)
        : this(uri, token, null)
    { }
    /// <summary>
    /// Creates a new Instance to represent a HomeAssitant server
    /// </summary>
    /// <param name="uri">Server's URI to conenct to</param>
    /// <param name="token">Token to authenticate as</param>
    /// <param name="handler">Http Handler to configure connections options as Proxy</param>
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
    /// <summary>
    /// Allows the additional configuration of the internal API Client
    /// </summary>
    public void ConfigureInternalClient(Action<ClientInfo> action)
    {
        action(client);
    }

    /// <summary>
    /// Checks if the inntance is responsive, intally uses IsRunningAsync()
    /// </summary>
    public async Task<bool> CheckRunningAsync()
    {
        try
        {
            await IsRunningAsync();
            return true;
        }
        catch { return false; }
    }
    /// <summary>
    /// Calls /api to check if the instance is responsive
    /// </summary>
    public async Task IsRunningAsync()
    {
        var info = await client.GetAsync<string>("/api/");
        if (!info.IsSuccessStatusCode) throw new Exceptions.ClientException($"Request to `/api/` Failed with code {info.StatusCode}", info);
    }
    /// <summary>
    /// Gets nstance Core State
    /// </summary>
    public async Task<Models.CoreStateModel> GetCoreState()
    {
        var info = await client.GetAsync<Models.CoreStateModel>("/api/core/state");
        if (!info.IsSuccessStatusCode) throw new Exceptions.ClientException($"Request to `/api/core/state` Failed with code {info.StatusCode}", info);

        return info.Data;
    }
    /// <summary>
    /// Requests error logs
    /// </summary>
    public async Task<string> ErrorLogsAsync()
    {
        var info = await client.GetAsync<string>("/api/error_log");
        if (!info.IsSuccessStatusCode) throw new Exceptions.ClientException($"Request to `/api/error_log` Failed with code {info.StatusCode}", info);

        return info.Data;
    }
}
/// <summary>
/// Extension class to add Fluent to Instance
/// </summary>
public static class InstanceExtension
{
    /// <summary>
    /// Gets the Source for the instance
    /// </summary>
    public static TSource Get<TSource>(this Instance instance)
        where TSource : SourceBase
    {
        return (TSource)Activator.CreateInstance(typeof(TSource), instance.client);
    }
}
