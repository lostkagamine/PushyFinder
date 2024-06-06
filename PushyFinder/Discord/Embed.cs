using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;

namespace PushyFinder.Discord
{
    public class Embed
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? Timestamp { get; set; }
        public uint? Color { get; set; }
        public Footer? Footer { get; set; }
        public Image? Image { get; set; }
        public Image? Thumbnail { get; set; }
        public Image? Video { get; set; }
        public Provider? Provider { get; set; }
        public Author? Author { get; set; }
        public List<Field>? Fields { get; set; }
    }

    public class Footer
    {
        public string Text { get; set; } = string.Empty;
        public string? IconUrl { get; set; }
        public string? ProxyIconUrl { get; set; }
    }

    public class Image
    {
        public string Url { get; set; } = string.Empty;
        public string? ProxyUrl { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }
    }

    public class Provider
    {
        public string? Name { get; set; }
        public string? Url { get; set; }
    }

    public class Author
    {
        public string Name { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? IconUrl { get; set; }
        public string? ProxyIconUrl { get; set; }
    }

    public class Field
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool? Inline { get; set; }
    }

    public class EmbedBuilder
    {
        private readonly Embed _embed = new();

        public EmbedBuilder WithTitle(string title)
        {
            _embed.Title = title;
            return this;
        }

        public EmbedBuilder WithDescription(string description)
        {
            _embed.Description = description;
            return this;
        }

        public EmbedBuilder WithUrl(string url)
        {
            _embed.Url = url;
            return this;
        }

        public EmbedBuilder WithTimestamp(DateTimeOffset timestamp)
        {
            _embed.Timestamp = timestamp.ToString("yyyy-MM-ddTHH:mm:ssZ");
            return this;
        }

        public EmbedBuilder WithColor(string colorString)
        {
            var index = colorString.IndexOf('#') + 1;
            var color = colorString[index..];
            if (color.Length != 6)
            {
                throw new ArgumentException("Color must be in the format RRGGBB");
            }

            return WithColor(uint.Parse(color, NumberStyles.AllowHexSpecifier));
        }

        public EmbedBuilder WithColor(Vector4 color)
        {
            var colorInt = (uint)(color.X * 255) << 16;
            colorInt += (uint)(color.Y * 255) << 8;
            colorInt += (uint)(color.Z * 255);
            return WithColor(colorInt);
        }

        public EmbedBuilder WithColor(uint color)
        {
            _embed.Color = color;
            return this;
        }

        public EmbedBuilder WithFooter(string text, string? iconUrl = null, string? proxyIconUrl = null)
        {
            _embed.Footer = new Footer
            {
                Text = text,
                IconUrl = iconUrl,
                ProxyIconUrl = proxyIconUrl
            };
            return this;
        }

        public EmbedBuilder WithImage(string url, string? proxyUrl = null, int? height = null, int? width = null)
        {
            _embed.Image = new Image
            {
                Url = url,
                ProxyUrl = proxyUrl,
                Height = height,
                Width = width
            };
            return this;
        }

        public EmbedBuilder WithThumbnail(string url, string? proxyUrl = null, int? height = null, int? width = null)
        {
            _embed.Thumbnail = new Image
            {
                Url = url,
                ProxyUrl = proxyUrl,
                Height = height,
                Width = width
            };
            return this;
        }

        public EmbedBuilder WithVideo(string url, string? proxyUrl = null, int? height = null, int? width = null)
        {
            _embed.Video = new Image
            {
                Url = url,
                ProxyUrl = proxyUrl,
                Height = height,
                Width = width
            };
            return this;
        }

        public EmbedBuilder WithProvider(string name, string? url = null)
        {
            _embed.Provider = new Provider
            {
                Name = name,
                Url = url
            };
            return this;
        }

        public EmbedBuilder WithAuthor(
            string name, string? url = null, string? iconUrl = null, string? proxyIconUrl = null)
        {
            _embed.Author = new Author
            {
                Name = name,
                Url = url,
                IconUrl = iconUrl,
                ProxyIconUrl = proxyIconUrl
            };
            return this;
        }

        public EmbedBuilder WithField(string name, string value, bool inline = false)
        {
            _embed.Fields ??= [];

            if (_embed.Fields.Count < 25)
            {
                _embed.Fields.Add(new Field
                {
                    Name = name,
                    Value = value,
                    Inline = inline
                });
            }
            else
            {
                throw new InvalidOperationException("Field limit reached");
            }
            return this;
        }

        public Embed Build()
        {
            return _embed;
        }
    }
}
