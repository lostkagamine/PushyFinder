using System;
using System.Collections.Generic;
using System.Linq;

namespace PushyFinder.Discord
{
    public class AllowedMentions
    {
        public List<string>? Parse { get; set; }
        public List<ulong>? Roles { get; set; }
        public List<ulong>? Users { get; set; }
        public bool? RepliedUser { get; set; }
    }

    public class AllowedMentionsBuilder
    {
        private readonly AllowedMentions _allowedMentions = new();

        public AllowedMentionsBuilder WithParse(params string[] parse)
        {
            _allowedMentions.Parse = parse.ToList();
            return this;
        }

        public AllowedMentionsBuilder WithRoles(params ulong[] roles)
        {
            if (roles.Length > 100)
            {
                throw new ArgumentException("You can only mention up to 100 roles.");
            }
            _allowedMentions.Roles = roles.ToList();
            return this;
        }

        public AllowedMentionsBuilder WithUsers(params ulong[] users)
        {
            if (users.Length > 100)
            {
                throw new ArgumentException("You can only mention up to 100 users.");
            }
            _allowedMentions.Users = users.ToList();
            return this;
        }

        public AllowedMentionsBuilder WithRepliedUser(bool repliedUser)
        {
            _allowedMentions.RepliedUser = repliedUser;
            return this;
        }

        public AllowedMentions Build()
        {
            return _allowedMentions;
        }
    }
}
