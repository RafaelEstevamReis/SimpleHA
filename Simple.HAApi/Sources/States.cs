using Simple.API;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple.HAApi.Sources
{
    public class States : SourceBase
    {
        public States(ClientInfo info)
            : base(info)
        { }

        public async Task<IEnumerable<Models.StateModel>> GetStates()
            => await GetAsync<Models.StateModel[]>("/api/states");

        public async Task<Models.StateModel> GetState(string entityId) 
            => await GetAsync<Models.StateModel>($"/api/states/{entityId}");
    }
}
