namespace Simple.HAApi.Sources;

using Simple.API;
using System.Threading.Tasks;

public class Template: SourceBase
{
    public Template(ClientInfo info)
        : base(info)
    { }

    public async Task<string> RenderTemplateAsync(string template)
        => await PostAsync<string>("/api/template", new { template });

}
