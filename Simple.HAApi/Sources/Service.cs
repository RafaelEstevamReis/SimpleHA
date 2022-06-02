using Simple.API;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple.HAApi.Sources
{
    public class Service : SourceBase
    {
        public Service(ClientInfo info)
             : base(info)
        { }

        public async Task<Models.ServiceModel[]> GetServicesAsync()
            => await GetAsync<Models.ServiceModel[]>("/api/services");

        public async Task<IEnumerable<Models.StateModel>> CallServiceAsync(string serviceName, object fields = null)
        {
            var serviceNameParts = serviceName.Split('.');
            return await PostAsync<IEnumerable<Models.StateModel>>($"/api/services/{serviceNameParts[0]}/{serviceNameParts[1]}", fields);
        }

        public async Task<IEnumerable<Models.StateModel>> CallServiceForEntitiesAsync(string serviceName, params string[] entityIds)
        {
            var serviceNameParts = serviceName.Split('.');
            return await PostAsync<IEnumerable<Models.StateModel>>($"/api/services/{serviceNameParts[0]}/{serviceNameParts[1]}", new { entity_id = entityIds });
        }
    }
}
