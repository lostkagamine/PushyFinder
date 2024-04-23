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
        
        // 17 = AFK, 18 = Camera Mode (should catch idle camera. also has the effect of catching gpose!)
        return Service.ClientState.LocalPlayer.OnlineStatus.Id is 17 or 18;
    }
    
    public static bool IsClientAfkDc()
    {

        if (Plugin.Configuration.IgnoreAfkStatusDc)
            return true;



        if (!Service.ClientState.IsLoggedIn ||
            Service.ClientState.LocalPlayer == null)
            return false;
        
        // 17 = AFK, 18 = Camera Mode (should catch idle camera. also has the effect of catching gpose!)
        return Service.ClientState.LocalPlayer.OnlineStatus.Id is 17 or 18;
    }
}
