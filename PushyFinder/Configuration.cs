using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace PushyFinder
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 1;

        public string PushoverAppKey { get; set; } = "";
        public string PushoverUserKey { get; set; } = "";
        public string PushoverDevice { get; set; } = "";
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
