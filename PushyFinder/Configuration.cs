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
        public bool IgnoreAfkStatus { get; set; } = false;
        
        public bool EnableForDutyPopsDc { get; set; } = true;
        public bool IgnoreAfkStatusDc { get; set; } = false;
        
        public bool EnablePushover { get; set; } = true;
        public bool EnableDiscord { get; set; } = true;
        
        public string DcHook { get; set; } = "";

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
