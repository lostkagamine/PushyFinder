using System;
using System.Collections.Generic;
using System.Linq;

namespace PushyFinder.Discord;

internal class Webhook
{
    public string Content { get; set; }
    public string? Username { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Tts { get; set; }
    public List<Embed>? Embeds { get; set; }
    public AllowedMentions? AllowedMentions { get; set; }

    /// <summary>
    ///     Not implemented
    /// </summary>
    public List<Components>? Components { get; set; }

    /// <summary>
    ///     Not implemented
    /// </summary>
    public List<Attachments>? Attachments { get; set; }

    /// <summary>
    ///     Only allows the following flags:
    ///     <value>SUPPRESS_EMBEDS</value>
    ///     <value>SUPPRESS_NOTIFICATIONS</value>
    /// </summary>
    public MessageFlags Flags { get; set; }

    public string? ThreadName { get; set; }
    public List<ulong>? AppliedTags { get; set; }

    /// <summary>
    ///     Not implemented
    /// </summary>
    public Poll? Poll { get; set; }
}

[Flags]
public enum MessageFlags
{
    CROSSPOSTED = 1 << 0,
    IS_CROSSPOST = 1 << 1,
    SUPPRESS_EMBEDS = 1 << 2,
    SOURCE_MESSAGE_DELETED = 1 << 3,
    URGENT = 1 << 4,
    HAS_THREAD = 1 << 5,
    EPHEMERAL = 1 << 6,
    LOADING = 1 << 7,
    FAILED_TO_MENTION_SOME_ROLES_IN_THREAD = 1 << 8,
    SUPPRESS_NOTIFICATIONS = 1 << 12,
    IS_VOICE_MESSAGE = 1 << 13
}

public class WebhookBuilder
{
    private readonly Webhook _webhook = new();

    public WebhookBuilder WithContent(string content)
    {
        _webhook.Content = content;
        return this;
    }

    public WebhookBuilder WithUsername(string username)
    {
        _webhook.Username = username;
        return this;
    }

    public WebhookBuilder WithAvatarUrl(string avatarUrl)
    {
        _webhook.AvatarUrl = avatarUrl;
        return this;
    }

    public WebhookBuilder WithTts(string tts)
    {
        _webhook.Tts = tts;
        return this;
    }

    public WebhookBuilder WithEmbed(EmbedBuilder builder)
    {
        _webhook.Embeds ??= new List<Embed>();

        if (_webhook.Embeds.Count < 10)
            _webhook.Embeds.Add(builder.Build());
        else
            throw new InvalidOperationException("Embed limit reached");
        return this;
    }

    public WebhookBuilder WithAllowedMentions(AllowedMentionsBuilder builder)
    {
        _webhook.AllowedMentions = builder.Build();
        return this;
    }

    public WebhookBuilder WithFlags(MessageFlags flags)
    {
        if ((flags & ~(MessageFlags.SUPPRESS_EMBEDS | MessageFlags.SUPPRESS_NOTIFICATIONS)) != 0)
            throw new ArgumentException("Invalid flags");
        _webhook.Flags = flags;
        return this;
    }

    public WebhookBuilder WithThreadName(string threadName)
    {
        _webhook.ThreadName = threadName;
        return this;
    }

    public WebhookBuilder WithAppliedTags(List<ulong> appliedTags)
    {
        _webhook.AppliedTags = appliedTags;
        return this;
    }

    /// <summary>
    ///     Builds the object to be sent to Discord
    /// </summary>
    /// <returns>The Discord JSON object</returns>
    public object Build()
    {
        // TODO: implement conversion from C# object to Discord JSON object
        return new
        {
            content = _webhook.Content,
            username = _webhook.Username,
            avatar_url = _webhook.AvatarUrl,
            tts = _webhook.Tts,
            embeds = _webhook.Embeds?.Select(t => t.ToJson()).ToArray(),
            allowed_mentions = _webhook.AllowedMentions?.ToJson(),
            flags = _webhook.Flags,
            thread_name = _webhook.ThreadName,
            applied_tags = _webhook.AppliedTags?.Select(t => t.ToString()).ToArray()
        };
    }
}
