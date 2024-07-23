using System;
using System.Collections.Generic;
using Dalamud.Configuration;
using Dalamud.Plugin;

namespace PushyFinder;

[Serializable]
public class Configuration : IPluginConfiguration
{
    // the below exist just to make saving less cumbersome
    [NonSerialized]
    private IDalamudPluginInterface? PluginInterface;

    public string PushoverAppKey { get; set; } = "";
    public string PushoverUserKey { get; set; } = "";
    public string PushoverDevice { get; set; } = "";
    public string PushoverSound { get; set; } = "";
    public string DiscordWebhookToken { get; set; } = "";
    public bool EnableForDutyPops { get; set; } = true;
    public bool IgnoreAfkStatus { get; set; } = false;
    public bool DiscordUseEmbed { get; set; } = true;
    public uint DiscordEmbedColor { get; set; } = 0x00FF00;
    public int Version { get; set; } = 1;
    public List<string> PushoverSoundList { get; set; } = new List<string>();
    public int PushoverSoundIndex { get; set; } = 0;
    public void Initialize(IDalamudPluginInterface pluginInterface)
    {
        PluginInterface = pluginInterface;
    }

    public void Save()
    {
        PluginInterface!.SavePluginConfig(this);
    }
}
