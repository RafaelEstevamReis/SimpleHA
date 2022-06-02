﻿using Simple.API;
using System.Threading.Tasks;

namespace Simple.HAApi.Sources
{
    public class Configuration : SourceBase
    {
        public Configuration(ClientInfo info)
            : base(info)
        { }

        public async Task<Models.ConfigurationModel> GetConfigurationAsync() 
            => await GetAsync<Models.ConfigurationModel>("/api/config");

        public async Task<Models.ConfigurationEntriesModel[]> GetConfigurationEntriesAsync(string type)
            => await GetAsync<Models.ConfigurationEntriesModel[]>($"api/config/config_entries/entry?type={type}");
        public async Task<Models.ConfigurationEntriesModel[]> GetConfigurationEntriesAsync()
            => await GetAsync<Models.ConfigurationEntriesModel[]>($"api/config/config_entries/entry?");

    }
}
