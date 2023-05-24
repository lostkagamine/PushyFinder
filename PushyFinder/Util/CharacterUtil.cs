using Dalamud.Game.ClientState;

namespace PushyFinder.Util;

public static class CharacterUtil
{
    public static bool IsClientAfk()
    {
        if (Plugin.Configuration.IgnoreAfkStatus)
            return true;
        
        if (!Service.ClientState.IsLoggedIn ||
            Service.ClientState.LocalPlayer == null)
            return false;
        
        // 13 = AFK, 14 = Camera Mode (should catch idle camera. also has the effect of catching gpose!)
        return Service.ClientState.LocalPlayer.OnlineStatus.Id is 13 or 14;
    }
}
