using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace PushyFinder;

public class Service
{
    [PluginService]
    public static DalamudPluginInterface PluginInterface { get; private set; } = null!;

    [PluginService]
    public static CommandManager CommandManager { get; private set; } = null!;

    [PluginService]
    public static ClientState ClientState { get; private set; } = null!;

    [PluginService]
    public static PartyList PartyList { get; private set; } = null!;

    [PluginService]
    public static Framework Framework { get; private set; } = null!;
    
    [PluginService]
    public static ChatGui ChatGui { get; private set; } = null!;
}
