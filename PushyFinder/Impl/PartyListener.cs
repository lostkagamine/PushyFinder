using System.Linq;
using Dalamud.Game.ClientState.Party;
using Dalamud.Logging;
using Lumina.Excel.GeneratedSheets;
using PushyFinder.Delivery;
using PushyFinder.Util;

namespace PushyFinder.Impl;

public static class PartyListener
{
    public static void On()
    {
        Service.PluginLog.Debug("PartyListener On");
        CrossWorldPartyListSystem.OnJoin += OnJoin;
        CrossWorldPartyListSystem.OnLeave += OnLeave;
    }
    
    public static void Off()
    {
        Service.PluginLog.Debug("PartyListener Off");
        CrossWorldPartyListSystem.OnJoin -= OnJoin;
        CrossWorldPartyListSystem.OnLeave -= OnLeave;
    }

    private static void OnJoin(CrossWorldPartyListSystem.CrossWorldMember m)
    {
        if (Plugin.Configuration.EnablePushover)
        {
            if (!CharacterUtil.IsClientAfk()) return;

            var jobAbbr = LuminaDataUtil.GetJobAbbreviation(m.JobId);

            if (m.PartyCount == 8)
            {
                PushoverDelivery.Deliver("Party full",
                                         $"{m.Name} (Lv{m.Level} {jobAbbr}) joins the party.\nParty recruitment ended. All spots have been filled.");
            }
            else
            {
                PushoverDelivery.Deliver($"{m.PartyCount}/8: Party join",
                                         $"{m.Name} (Lv{m.Level} {jobAbbr}) joins the party.");
            }
        }

        if (Plugin.Configuration.EnableDiscord)
        {
            if (!CharacterUtil.IsClientAfk()) return;

            var jobAbbr = LuminaDataUtil.GetJobAbbreviation(m.JobId);

            if (m.PartyCount == 8)
            {
                DncDelivery.Deliver("Party full",
                                         $"{m.Name} (Lv{m.Level} {jobAbbr}) joins the party.\nParty recruitment ended. All spots have been filled.");
            }
            else
            {
                DncDelivery.Deliver($"{m.PartyCount}/8: Party join",
                                         $"{m.Name} (Lv{m.Level} {jobAbbr}) joins the party.");
            }
        }
    }
    
    private static void OnLeave(CrossWorldPartyListSystem.CrossWorldMember m)
    {
        if (Plugin.Configuration.EnablePushover)
        {
            if (!CharacterUtil.IsClientAfk()) return;
        
            var jobAbbr = LuminaDataUtil.GetJobAbbreviation(m.JobId);

            PushoverDelivery.Deliver($"{m.PartyCount-1}/8: Party leave",
                                     $"{m.Name} (Lv{m.Level} {jobAbbr}) has left the party.");
        }

        if (Plugin.Configuration.EnableDiscord)
        {
            if (!CharacterUtil.IsClientAfk()) return;
        
            var jobAbbr = LuminaDataUtil.GetJobAbbreviation(m.JobId);

            DncDelivery.Deliver($"{m.PartyCount-1}/8: Party leave",
                                     $"{m.Name} (Lv{m.Level} {jobAbbr}) has left the party.");
        }
    }
}
