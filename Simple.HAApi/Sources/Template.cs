using Simple.API;
using System.Threading.Tasks;

namespace Simple.HAApi.Sources
{
    public class Template: SourceBase
    {
        public Template(ClientInfo info)
            : base(info)
        { }

        public async Task<string> RenderTemplateAsync(string template)
            => await PostAsync<string>("/api/template", new { template });

    }
}
