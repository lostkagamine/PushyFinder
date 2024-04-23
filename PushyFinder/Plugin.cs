using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using Dalamud.Plugin.Services;
using PushyFinder.Impl;
using PushyFinder.Util;
using PushyFinder.Windows;

namespace PushyFinder
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "PushyFinder";
        private const string CommandName = "/pf";

        private DalamudPluginInterface PluginInterface { get; init; }
        private ICommandManager CommandManager { get; init; }
        
        // This *is* used.
#pragma warning disable CS8618
        public static Configuration Configuration { get; private set; }
#pragma warning restore
        
        public WindowSystem WindowSystem = new("PushyFinder");

        private ConfigWindow ConfigWindow { get; init; }

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] ICommandManager commandManager)
        {
            pluginInterface.Create<Service>();
            
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Configuration.Initialize(this.PluginInterface);
            
            ConfigWindow = new ConfigWindow(this);
            
            WindowSystem.AddWindow(ConfigWindow);

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Opens settings\n'pt' toggles Pushover whether it's enabled.\n'pon' enables the Pushover plugin\n'poff' disables the Pushover plugin.\n replace 'p' with 'd' for the Discord side"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;

            CrossWorldPartyListSystem.Start();
            PartyListener.On();
            DutyListener.On();
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            
            ConfigWindow.Dispose();

            CrossWorldPartyListSystem.Stop();
            PartyListener.Off();
            DutyListener.Off();

            this.CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            
            switch (args.Trim())
            {
                case "pt" or "toggle":
                    Configuration.EnablePushover = !Configuration.EnablePushover;
                    Service.ChatGui.Print($"Pushover plugin {(Configuration.EnablePushover ? "enabled" : "disabled")}.");
                    break;
                case "pon":
                    Configuration.EnablePushover = true;
                    Service.ChatGui.Print($"Pushover plugin enabled.");
                    break;
                case "poff":
                    Configuration.EnablePushover = false;
                    Service.ChatGui.Print($"Pushover plugin disabled.");
                    break;
                case "dt" or "toggle":
                    Configuration.EnableDiscord = !Configuration.EnableDiscord;
                    Service.ChatGui.Print($"Discord Webhook plugin {(Configuration.EnableDiscord ? "enabled" : "disabled")}.");
                    break;
                case "don":
                    Configuration.EnableDiscord = true;
                    Service.ChatGui.Print($"Discord Webhook plugin enabled.");
                    break;
                case "doff":
                    Configuration.EnableDiscord = false;
                    Service.ChatGui.Print($"Discord Webhook plugin disabled.");
                    break;
                case "":
                    ConfigWindow.IsOpen = true;
                    break;
            }
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            ConfigWindow.IsOpen = true;
        }
    }
}
