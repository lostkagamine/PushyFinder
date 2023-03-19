using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace PushyFinder.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration Configuration;
    
    public ConfigWindow(Plugin plugin) : base(
        "PushyFinder Configuration",
        ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
        ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.AlwaysAutoResize)
    {
        Configuration = Plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw()
    {
        {
            var cfg = Configuration.PushoverAppKey;
            if (ImGui.InputText("Application key", ref cfg, 2048u))
            {
                Configuration.PushoverAppKey = cfg;
            }
        }
        {
            var cfg = Configuration.PushoverUserKey;
            if (ImGui.InputText("User key", ref cfg, 2048u))
            {
                Configuration.PushoverUserKey = cfg;
            }
        }
        {
            var cfg = Configuration.PushoverDevice;
            if (ImGui.InputText("Device name", ref cfg, 2048u))
            {
                Configuration.PushoverDevice = cfg;
            }
        }
        {
            var cfg = Configuration.EnableForDutyPops;
            if (ImGui.Checkbox("Send message for duty pop?", ref cfg))
            {
                Configuration.EnableForDutyPops = cfg;
            }
        }
        
        if (ImGui.Button("Save and close"))
        {
            Configuration.Save();
            IsOpen = false;
        }
    }
}
