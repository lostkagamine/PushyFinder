using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using PushyFinder.Impl;
using PushyFinder.Util;
using PushyFinder.Windows;

namespace PushyFinder;

public sealed class Plugin : IDalamudPlugin
{
    public string Name => "PushyFinder";
    private const string CommandName = "/pushyfinder";

    private IDalamudPluginInterface PluginInterface { get; init; }
    private ICommandManager CommandManager { get; init; }

    // This *is* used.
#pragma warning disable CS8618
    public static Configuration Configuration { get; private set; }
#pragma warning restore

    public WindowSystem WindowSystem = new("PushyFinder");

    private ConfigWindow ConfigWindow { get; init; }

    public Plugin(
        IDalamudPluginInterface pluginInterface,
        ICommandManager commandManager)
    {
        pluginInterface.Create<Service>();

        PluginInterface = pluginInterface;
        CommandManager = commandManager;

        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Configuration.Initialize(PluginInterface);

        ConfigWindow = new ConfigWindow(this);

        WindowSystem.AddWindow(ConfigWindow);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Opens the configuration window."
        });

        PluginInterface.UiBuilder.Draw += DrawUI;
        PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;

        CrossWorldPartyListSystem.Start();
        PartyListener.On();
        DutyListener.On();
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();

        CrossWorldPartyListSystem.Stop();
        PartyListener.Off();
        DutyListener.Off();

        CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args)
    {
        if (args == "debugOnlineStatus")
        {
            Service.ChatGui.Print($"OnlineStatus ID = {Service.ClientState.LocalPlayer!.OnlineStatus.RowId}");
            return;
        }

        ConfigWindow.IsOpen = true;
    }

    private void DrawUI()
    {
        WindowSystem.Draw();
    }

    public void DrawConfigUI()
    {
        ConfigWindow.IsOpen = true;
    }
}
