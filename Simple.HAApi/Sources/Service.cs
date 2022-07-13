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

        public async Task<IEnumerable<Models.StateModel>> CallServiceAsync(string serviceName, params string[] entityIds)
        {
            var serviceNameParts = serviceName.Split('.');
            return await PostAsync<IEnumerable<Models.StateModel>>($"/api/services/{serviceNameParts[0]}/{serviceNameParts[1]}", new { entity_id = entityIds });
        }

        public async Task<IEnumerable<Models.StateModel>> CallServiceJsonAsync(string serviceName, string json)
        {
            var serviceNameParts = serviceName.Split('.');
            return await PostJsonAsync<IEnumerable<Models.StateModel>>($"/api/services/{serviceNameParts[0]}/{serviceNameParts[1]}", json);
        }


        /// <summary>
        /// https://www.home-assistant.io/docs/automation/services/
        /// </summary>
        public static OnOff Automation { get; } = new("automation");
        /// <summary>
        /// https://www.home-assistant.io/integrations/climate/
        /// </summary>
        public static OnOff Climate { get; } = new("climate");
        /// <summary>
        /// https://www.home-assistant.io/integrations/cover/#cover-control-services
        /// </summary>
        public static OpenCloseCover Cover { get; } = new();
        /// <summary>
        /// https://www.home-assistant.io/integrations/input_boolean/#services
        /// </summary>
        public static OnOff InputBoolean { get; } = new("input_boolean");
        /// <summary>
        /// https://www.home-assistant.io/integrations/light#service-lightturn_on
        /// </summary>
        public static OnOff Light { get; } = new("light");
        /// <summary>
        /// https://www.home-assistant.io/integrations/media_player/#media-control-services
        /// </summary>
        public static Player MediaPlayer { get; } = new();
        /// <summary>
        /// https://www.home-assistant.io/integrations/scene/
        /// </summary>
        public static OnOff Scene { get; } = new("scene");
        /// <summary>
        /// https://www.home-assistant.io/integrations/switch/#use-the-services
        /// </summary>
        public static OnOff Switch { get; } = new("switch");

        public class OnOff
        {
            public OnOff(string domainName)
            {
                TurnOn = $"{domainName}.turn_on";
                TurnOff = $"{domainName}.turn_off";
                Toggle = $"{domainName}.toggle";
            }

            public string TurnOn { get; }
            public string TurnOff { get; }
            public string Toggle { get; }
        }
        public class OpenCloseCover
        {
            public readonly string Open = "cover.open_cover";
            public readonly string Close = "cover.close_cover";
        }
        public class Player
        {
            public readonly string Play = "media_player.media_play";
            public readonly string Pause = "media_player.media_pause";
            public readonly string Stop = "media_player.media_stop";
            public readonly string Toggle = "media_player.toggle";
        }
    }
}
