using Simple.API;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Simple.HAApi
{
    public abstract class SourceBase
    {
        protected ClientInfo client { get; private set; }
        protected SourceBase(ClientInfo info)
        {
            client = info;
        }

        protected async Task<T> GetAsync<T>(string path)
        {
            var info = await client.GetAsync<T>(path);
            if (!info.IsSuccessStatusCode) throw new Exceptions.ClientException($"Request to `{path}` Failed with code {info.StatusCode}", info);

            return info.Data;
        }
        protected async Task<T> PostAsync<T>(string path, object data)
        {
            var info = await client.PostAsync<T>(path, data);
            if (!info.IsSuccessStatusCode) throw new Exceptions.ClientException($"Request to `{path}` Failed with code {info.StatusCode}", info);

            return info.Data;
        }
        protected async Task<T> PostJsonAsync<T>(string path, string json)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var info = await client.PostAsync<T>(path, content);
            if (!info.IsSuccessStatusCode) throw new Exceptions.ClientException($"Request to `{path}` Failed with code {info.StatusCode}", info);

            return info.Data;
        }

    }
}
