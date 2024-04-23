using Dalamud.Utility;
using Lumina.Excel.GeneratedSheets;
using PushyFinder.Delivery;
using PushyFinder.Util;

namespace PushyFinder.Impl;

public class DutyListener
{
    public static void On()
    {
        Service.PluginLog.Debug("DutyListener On");
        Service.ClientState.CfPop += OnDutyPop;
    }

    public static void Off()
    {
        Service.PluginLog.Debug("DutyListener Off");
        Service.ClientState.CfPop -= OnDutyPop;
    }

    private static void OnDutyPop(ContentFinderCondition e)
    {
        if (Plugin.Configuration.EnablePushover)
        {
            if (!Plugin.Configuration.EnableForDutyPops)
                return;

            if (!CharacterUtil.IsClientAfk())
                return;
        
            var dutyName = e.RowId == 0 ? "Duty Roulette" : e.Name.ToDalamudString().TextValue;
            PushoverDelivery.Deliver($"Duty pop", $"Duty registered: '{dutyName}'.");
        }

        if (Plugin.Configuration.EnableDiscord)
        {
            if (!Plugin.Configuration.EnableForDutyPopsDc)
                return;

            if (!CharacterUtil.IsClientAfkDc())
                return;
        
            var dutyName = e.RowId == 0 ? "Duty Roulette" : e.Name.ToDalamudString().TextValue;
            DncDelivery.Deliver($"Duty pop", $"Duty registered: '{dutyName}'.");
        }
    }
}
