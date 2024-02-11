using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace PushyFinder;

public class Service
{
    [PluginService]
    public static DalamudPluginInterface PluginInterface { get; private set; } = null!;

    [PluginService]
    public static ICommandManager CommandManager { get; private set; } = null!;

    [PluginService]
    public static IClientState ClientState { get; private set; } = null!;

    [PluginService]
    public static IPartyList PartyList { get; private set; } = null!;

    [PluginService]
    public static IFramework Framework { get; private set; } = null!;
    
    [PluginService]
    public static IChatGui ChatGui { get; private set; } = null!;
    
    [PluginService]
    public static IDataManager DataManager { get; private set; } = null!;

    [PluginService]
    public static IPluginLog PluginLog { get; private set; } = null!;
}
