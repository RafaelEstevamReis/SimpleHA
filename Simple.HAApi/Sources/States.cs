namespace Simple.HAApi.Sources;

using Simple.API;
using System.Collections.Generic;
using System.Threading.Tasks;

public class States : SourceBase
{
    public States(ClientInfo info)
        : base(info)
    { }

    public async Task<IEnumerable<Models.StateModel>> GetStatesAsync()
        => await GetAsync<Models.StateModel[]>("/api/states");

    public async Task<Models.StateModel> GetStateAsync(string entityId) 
        => await GetAsync<Models.StateModel>($"/api/states/{entityId}");
}
