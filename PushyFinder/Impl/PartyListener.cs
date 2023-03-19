using Dalamud.Game.ClientState.Party;
using Dalamud.Logging;
using PushyFinder.Delivery;
using PushyFinder.Util;

namespace PushyFinder.Impl;

public static class PartyListener
{
    public static void On()
    {
        CrossWorldPartyListSystem.OnJoin += OnJoin;
        CrossWorldPartyListSystem.OnLeave += OnLeave;
    }
    
    public static void Off()
    {
        CrossWorldPartyListSystem.OnJoin -= OnJoin;
        CrossWorldPartyListSystem.OnLeave -= OnLeave;
    }

    private static void OnJoin(CrossWorldPartyListSystem.CrossWorldMember m)
    {
        if (!CharacterUtil.IsClientAfk()) return;

        if (m.PartyCount == 8)
        {
            PushoverDelivery.Deliver("Party full",
                                     $"{m.Name} joins the party.\n\nParty recruitment ended. All spots have been filled.");
        }
        else
        {
            PushoverDelivery.Deliver("Party member joined",
                                     $"{m.Name} joins the party.\n\n{m.PartyCount}/8 members.");
        }
    }
    
    private static void OnLeave(CrossWorldPartyListSystem.CrossWorldMember m)
    {
        if (!CharacterUtil.IsClientAfk()) return;
        
        PushoverDelivery.Deliver("Party member left",
                                 $"{m.Name} leaves the party.\n\n{m.PartyCount}/8 members.");
    }
}
