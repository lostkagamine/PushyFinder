using System.Text.Json;
using System.Threading.Tasks;
using Dalamud.Utility;
using Flurl.Http;
using PushyFinder.Discord;

namespace PushyFinder.Delivery;

internal class DiscordDelivery : IDelivery
{
    public bool IsActive => !Plugin.Configuration.DiscordWebhookToken.IsNullOrWhitespace();

    public void Deliver(string title, string text)
    {
        Task.Run(() => DeliverAsync(title, text));
    }

    // very basic discord implementation to ride off the same delivery method could be extended later to be more feature rich if needed
    private static async void DeliverAsync(string title, string text)
    {
        var webhook = new WebhookBuilder();

        if (!Plugin.Configuration.DiscordUseEmbed)
            webhook.WithContent(title + "\n" + text);
        else
        {
            webhook.WithEmbed(new EmbedBuilder()
                              .WithDescription(text)
                              .WithTitle(title)
                              .WithColor(Plugin.Configuration.DiscordEmbedColor)
                              .WithAuthor("PushyFinder", "https://github.com/lostkagamine/PushyFinder",
                                          "https://raw.githubusercontent.com/goatcorp/PluginDistD17/main/stable/PushyFinder/images/icon.png"));
        }

        webhook.WithAvatarUrl(
            "https://raw.githubusercontent.com/goatcorp/PluginDistD17/main/stable/PushyFinder/images/icon.png");
        webhook.WithUsername("PushyFinder");

        try
        {
            // this can break if they register a webhook to a channel type of forum or media
            await Plugin.Configuration.DiscordWebhookToken.PostJsonAsync(webhook.Build());
            Service.PluginLog.Debug("Sent Discord message");
        }
        catch (FlurlHttpException e)
        {
            // Discord returns a json object within the message that contains the error not sure if this is something that should be parsed or not
            Service.PluginLog.Error($"Failed to make Discord request: '{e.Message}'");
            Service.PluginLog.Error($"{e.StackTrace}");
            Service.PluginLog.Debug(JsonSerializer.Serialize(webhook.Build()));
        }
    }
}
