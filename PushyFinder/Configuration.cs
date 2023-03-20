using Dalamud.Configuration;
using Dalamud.Plugin;
using PushyFinder.Delivery;
using System;

namespace PushyFinder
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 1;

        public deliveries DeliveryService { get; set; } = deliveries.Pushover;

        public string PushoverAppKey { get; set; } = "";
        public string PushoverUserKey { get; set; } = "";
        public string PushoverDevice { get; set; } = "";

        public string ntfyTopic { get; set; } = "";
        public string ntfyDomain { get; set; } = "https://ntfy.sh/";

        public bool EnableForDutyPops { get; set; } = true;

        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private DalamudPluginInterface? PluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.PluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.PluginInterface!.SavePluginConfig(this);
        }
    }
}
