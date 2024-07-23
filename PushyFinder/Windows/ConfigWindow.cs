using System;
using System.Numerics;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using PushyFinder.Delivery;
using PushyFinder.Util;
using PushyFinder.Retrieve;

namespace PushyFinder.Windows;

public class ConfigWindow : Window, IDisposable
{
    private readonly Configuration Configuration;

    private readonly TimedBool notifSentMessageTimer = new(3.0f);

    public ConfigWindow(Plugin plugin) : base(
        "PushyFinder Configuration",
        ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
        ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.AlwaysAutoResize)
    {
        Configuration = Plugin.Configuration;
    }

    public void Dispose() { }

    private void DrawPushoverConfig()
    {
        {
            var cfg = Configuration.PushoverAppKey;
            if (ImGui.InputText("Application key", ref cfg, 2048u)) Configuration.PushoverAppKey = cfg;
        }
        {
            var cfg = Configuration.PushoverUserKey;
            if (ImGui.InputText("User key", ref cfg, 2048u)) Configuration.PushoverUserKey = cfg;
        }
        {
            var cfg = Configuration.PushoverDevice;
            if (ImGui.InputText("Device name", ref cfg, 2048u)) Configuration.PushoverDevice = cfg;
        }
        if (Configuration.PushoverSoundList.Count > 0)
        {
            int soundIndex = Configuration.PushoverSoundIndex;
            if (ImGui.Combo("Sound", ref soundIndex, Configuration.PushoverSoundList.ToArray(), Configuration.PushoverSoundList.Count))
            {
                Configuration.PushoverSoundIndex = soundIndex;
                Configuration.PushoverSound = Configuration.PushoverSoundList[Configuration.PushoverSoundIndex];
            }
        }
        {
            if (ImGui.Button("Refresh sound list")) PushoverRetrieve.GetSounds();
        }
        {
            var cfg = Configuration.EnableForDutyPops;
            if (ImGui.Checkbox("Send message for duty pop?", ref cfg)) Configuration.EnableForDutyPops = cfg;
        }
    }

    private void DrawDiscordConfig()
    {
        {
            var cfg = Configuration.DiscordWebhookToken;
            if (ImGui.InputText("Webhook token", ref cfg, 2048u)) Configuration.DiscordWebhookToken = cfg;
        }
        {
            var cfg = Configuration.DiscordUseEmbed;
            if (ImGui.Checkbox("Use embeds?", ref cfg)) Configuration.DiscordUseEmbed = cfg;
        }
        {
            var vec3Col = new Vector3();
            var cfg = Configuration.DiscordEmbedColor;
            vec3Col.X = ((cfg >> 16) & 0xFF) / 255.0f;
            vec3Col.Y = ((cfg >> 8) & 0xFF) / 255.0f;
            vec3Col.Z = (cfg & 0xFF) / 255.0f;
            if (ImGui.ColorEdit3("Embed color", ref vec3Col))
            {
                cfg = ((uint)(vec3Col.X * 255) << 16) | ((uint)(vec3Col.Y * 255) << 8) | (uint)(vec3Col.Z * 255);
                Configuration.DiscordEmbedColor = cfg;
            }
        }
    }

    public override void Draw()
    {
        using (var tabBar = ImRaii.TabBar("Services"))
        {
            if (tabBar)
            {
                using (var pushoverTab = ImRaii.TabItem("Pushover"))
                {
                    if (pushoverTab) DrawPushoverConfig();
                }

                using (var discordTab = ImRaii.TabItem("Discord"))
                {
                    if (discordTab) DrawDiscordConfig();
                }
            }
        }

        ImGui.NewLine();

        if (ImGui.Button("Send test notification"))
        {
            notifSentMessageTimer.Start();
            MasterDelivery.Deliver("Test notification",
                                   "If you received this, PushyFinder is configured correctly.");
        }

        if (notifSentMessageTimer.Value)
        {
            ImGui.SameLine();
            ImGui.Text("Notification sent!");
        }

        {
            var cfg = Configuration.IgnoreAfkStatus;
            if (ImGui.Checkbox("Ignore AFK status and always notify", ref cfg)) Configuration.IgnoreAfkStatus = cfg;
        }

        if (!Configuration.IgnoreAfkStatus)
        {
            if (!CharacterUtil.IsClientAfk())
            {
                var red = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
                ImGui.TextColored(red, "This plugin will only function while your client is AFK (/afk, red icon)!");

                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("The reasoning for this is that if you are not AFK, you are assumed to");
                    ImGui.Text("be at your computer, and ready to respond to a join or a duty pop.");
                    ImGui.Text("Notifications would be bothersome, so they are disabled.");
                    ImGui.EndTooltip();
                }
            }
            else
            {
                var green = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
                ImGui.TextColored(green, "You are AFK. The plugin is active and notifications will be served.");
            }
        }

        if (ImGui.Button("Save and close"))
        {
            Configuration.Save();
            IsOpen = false;
        }
    }
}
