using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Dalamud.Logging;
using Flurl.Http;

namespace PushyFinder.Delivery;

public abstract class NtfyDelivery : IDelivery
{
    public static string Name => "Ntfy.sh";

    public static void Deliver(string title, string text = "")
    {
        if (Plugin.Configuration.ntfyTopic.Length == 0 ||
            Plugin.Configuration.ntfyDomain.Length == 0) return;
        
        Task.Run(() => DeliverAsync(title, text));
    }

    private static async void DeliverAsync(string title, string text)
    {
        try
        {
            var url = Plugin.Configuration.ntfyDomain + Plugin.Configuration.ntfyTopic;

            await url.WithHeader("Title", title).PostStringAsync(text);
        }
        catch (FlurlHttpException e)
        {
            PluginLog.Error($"Failed to make ntfy req: '{e.Message}'");
            PluginLog.Error($"{e.StackTrace}");
        }
    }
}
