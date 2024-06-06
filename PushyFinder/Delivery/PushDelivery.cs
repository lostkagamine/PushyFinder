using System.Collections.Generic;
using System.Threading.Tasks;
using Dalamud.Utility;
using Flurl.Http;
using PushyFinder.Discord;

namespace PushyFinder.Delivery;

public static class PushDelivery
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
        if (!Plugin.Configuration.PushoverAppKey.IsNullOrWhitespace() &&
            !Plugin.Configuration.PushoverUserKey.IsNullOrWhitespace() &&
            !Plugin.Configuration.PushoverDevice.IsNullOrWhitespace())
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

        // very basic discord implementation to ride off the same delivery method could be extended later to be more feature rich if needed
        if (!Plugin.Configuration.DiscordWebhookToken.IsNullOrWhitespace())
        {
            var webhook = new WebhookBuilder();

            if (!Plugin.Configuration.DiscordUseEmbed)
            {
                webhook.WithContent(title + "\n" + text);
            }
            else
            {
                webhook.WithEmbed(new EmbedBuilder()
                                  .WithDescription(text)
                                  .WithTitle(title)
                                  .WithColor(Plugin.Configuration.DiscordEmbedColor)
                                  .WithAuthor("PushyFinder", "https://github.com/lostkagamine/PushyFinder", "https://raw.githubusercontent.com/goatcorp/PluginDistD17/blob/main/stable/PushyFinder/images/icon.png"));
            }

            webhook.WithAvatarUrl("https://raw.githubusercontent.com/goatcorp/PluginDistD17/blob/main/stable/PushyFinder/images/icon.png");
            webhook.WithUsername("PushyFinder");

            try
            {
                // this can break if they register a webhook to a channel type of forum or media
                await Plugin.Configuration.DiscordWebhookToken.PostJsonAsync(webhook);
            }
            catch (FlurlHttpException e)
            {
                // Discord returns a json object within the message that contains the error not sure if this is something that should be parsed or not
                Service.PluginLog.Error($"Failed to make Discord request: '{e.Message}'");
                Service.PluginLog.Error($"{e.StackTrace}");
            }
        }
    }
}
