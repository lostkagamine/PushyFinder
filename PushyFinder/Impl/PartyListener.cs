using Dalamud.Game.ClientState.Party;
using Dalamud.Logging;
using PushyFinder.Util;

namespace PushyFinder.Impl;

public static class PartyListener
{
    public static void On()
    {
        PartyListSystem.OnJoin += OnJoin;
        PartyListSystem.OnLeave += OnLeave;
    }
    
    public static void Off()
    {
        PartyListSystem.OnJoin -= OnJoin;
        PartyListSystem.OnLeave -= OnLeave;
    }

    private static void OnJoin(PartyMember m)
    {
        var mname = m.Name.ToString();
        PluginLog.Information($"Member join: {mname}");
    }
    
    private static void OnLeave(PartyMember m)
    {
        var mname = m.Name.ToString();
        PluginLog.Information($"Member leave: {mname}");
    }
}
