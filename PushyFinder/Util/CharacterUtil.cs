using Dalamud.Game.ClientState;

namespace PushyFinder.Util;

public static class CharacterUtil
{
    public static bool IsClientAfk()
    {
        if (!Service.ClientState.IsLoggedIn ||
            Service.ClientState.LocalPlayer == null)
            return false;

        return Service.ClientState.LocalPlayer.OnlineStatus.Id == 17;
    }
}
