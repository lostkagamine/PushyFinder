using System.Collections.Generic;
using System.Threading.Tasks;
using Dalamud.Utility;
using Flurl.Http;

namespace PushyFinder.Delivery;

public class NtfyDelivery : IDelivery
{
    public bool IsActive => !Plugin.Configuration.NtfyServer.IsNullOrWhitespace() &&
                            !Plugin.Configuration.NtfyTopic.IsNullOrWhitespace();

    public void Deliver(string title, string text)
    {
        Task.Run(() => DeliverAsync(title, text));
    }

    private static async void DeliverAsync(string title, string text)
    {
        var args = new Dictionary<string, string>
        {
            { "topic", Plugin.Configuration.NtfyTopic },
            { "title", title },
            { "message", text },
            { "icon", "https://raw.githubusercontent.com/goatcorp/PluginDistD17/main/stable/PushyFinder/images/icon.png" }
        };

        var request = new FlurlRequest(Plugin.Configuration.NtfyServer);
        if (!Plugin.Configuration.NtfyToken.IsNullOrWhitespace())
            request = request.WithOAuthBearerToken(Plugin.Configuration.NtfyToken);

        try
        {
            await request.PostJsonAsync(args);
            Service.PluginLog.Debug("Sent Ntfy message");
        }
        catch (FlurlHttpException e)
        {
            Service.PluginLog.Error($"Failed to make Ntfy request: '{e.Message}'");
            Service.PluginLog.Error($"{e.StackTrace}");
        }
    }
}
