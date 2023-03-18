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
        if (!CharacterUtil.IsClientAfk()) return;

        var mname = m.Name.ToString();
        Service.ChatGui.Print($"AFK member join: {mname}");
    }
    
    private static void OnLeave(PartyMember m)
    {
        if (!CharacterUtil.IsClientAfk()) return;
        
        var mname = m.Name.ToString();
        Service.ChatGui.Print($"AFK member leave: {mname}");
    }
}
