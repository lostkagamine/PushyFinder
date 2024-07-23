using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Dalamud.Utility;
using Flurl.Http;

namespace PushyFinder.Retrieve
{
    public class PushoverRetrieve
    {
        public static readonly string PUSHOVER_API = "https://api.pushover.net/1/sounds.json?token=";
        public static void GetSounds()
        {
            var urlWithToken = $"{PUSHOVER_API}{Plugin.Configuration.PushoverAppKey}";
            Service.PluginLog.Info(urlWithToken);
            Task.Run(async () =>
            {
                var response = await urlWithToken.GetJsonAsync<PushoverSoundsResponse>();
                Plugin.Configuration.PushoverSoundList.Clear();
                foreach (var sound in response.Sounds)
                {
                    Plugin.Configuration.PushoverSoundList.Add(sound.Key);
                    Service.PluginLog.Info($"{sound.Key}: {sound.Value}");
                }
            });
        }

    }
}

