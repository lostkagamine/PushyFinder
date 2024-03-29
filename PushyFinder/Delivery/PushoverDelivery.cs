using System.Collections.Generic;
using System.Threading.Tasks;
using Dalamud.Logging;
using Flurl.Http;

namespace PushyFinder.Delivery;

public static class PushoverDelivery
{
    public static readonly string PUSHOVER_API = "https://api.pushover.net/1/messages.json";
    
    public static void Deliver(string title, string text = "")
    {
        if (Plugin.Configuration.PushoverAppKey.Length == 0 ||
            Plugin.Configuration.PushoverDevice.Length == 0 ||
            Plugin.Configuration.PushoverUserKey.Length == 0) return;
        
        Task.Run(() => DeliverAsync(title, text));
    }

    private static async void DeliverAsync(string title, string text)
    {
        var args = new Dictionary<string, string>
        {
            { "token", Plugin.Configuration.PushoverAppKey },
            { "user", Plugin.Configuration.PushoverUserKey },
            { "device", Plugin.Configuration.PushoverDevice },
            { "title", title },
            { "message", text }
        };

        try
        {
            await PUSHOVER_API.PostJsonAsync(args);
        }
        catch (FlurlHttpException e)
        {
            Service.PluginLog.Error($"Failed to make Pushover request: '{e.Message}'");
            Service.PluginLog.Error($"{e.StackTrace}");
        }
    }
}
