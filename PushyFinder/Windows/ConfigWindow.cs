using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using PushyFinder.Delivery;
using PushyFinder.Util;

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

    private TimedBool notifSentMessageTimer = new(3.0f);

    public override void Draw()
    {
        var service = Configuration.DeliveryService;
        if (ImGui.BeginCombo("Service", service.ToString()))
        {
            foreach (var item in Enum.GetValues<Deliveries>())
            {
                if (ImGui.Selectable(item.ToString(), Configuration.DeliveryService == item)) Configuration.DeliveryService = item;
            }
        }
        if (service == Deliveries.Pushover)
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
        }
        else if (service == Deliveries.Ntfy)
        {
            {
                var cfg = Configuration.NtfyTopic;
                if (ImGui.InputText("Topic", ref cfg, 2048u))
                {
                    Configuration.NtfyTopic = cfg;
                }
            }
            {
                var cfg = Configuration.NtfyDomain;
                if (ImGui.InputText("Domain", ref cfg, 2048u))
                {
                    Configuration.NtfyDomain = cfg;
                }
            }
        }
        {
            var cfg = Configuration.EnableForDutyPops;
            if (ImGui.Checkbox("Send message for duty pop?", ref cfg))
            {
                Configuration.EnableForDutyPops = cfg;
            }
        }

        if (ImGui.Button("Send test notification"))
        {
            notifSentMessageTimer.Start();
            DeliveryManager.Deliver().Invoke("Test notification", 
                                     "If you received this, PushyFinder is configured correctly.");
        }

        if (notifSentMessageTimer.Value)
        {
            ImGui.Text("Notification sent!");
        }
        
        if (ImGui.Button("Save and close"))
        {
            Configuration.Save();
            IsOpen = false;
        }
    }
}
