using System.Threading.Tasks;
using Dalamud.Logging;
using Flurl.Http;
using Newtonsoft.Json;

namespace PushyFinder.Delivery
{
public static class DncDelivery
{
    public static void Deliver(string title, string text = "")
    {
        Task.Run(() => DeliverAsync(title, text));
    }

    private static async Task DeliverAsync(string title, string text)
    {
        // Replace 'YOUR_DISCORD_WEBHOOK_URL' with your actual Discord webhook URL
        var discordWebhookUrl = Plugin.Configuration.DcHook;

        // Constructing the payload for Discord webhook
        var payload = new
        {
            username = "PushyFinder",
            avatar_url= "https://i.imgur.com/wAhXLxp.png",
            embeds = new[]
            {
                new
                {
                    title = title,
                    description = text,
                    color = 16711680 // You can change the color here (e.g., 16711680 is red)
                }
            }
            //content = title + " " + text
        };

        try
        {
            
            // Sending the payload to Discord webhook URL
            await discordWebhookUrl
                .PostJsonAsync(payload);
        }
        catch (FlurlHttpException e)
        {
            if (e.Call.Response != null)
            {
                PluginLog.Error($"Failed to send notification to Discord webhook. Status code: {e.Call.Response.StatusCode}, response body: {await e.GetResponseStringAsync()}");
            }
            else
            {
                PluginLog.Error($"Failed to send notification to Discord webhook: '{e.Message}'");
                PluginLog.Error($"{e.StackTrace}");
            }
        }
    }
}
}
