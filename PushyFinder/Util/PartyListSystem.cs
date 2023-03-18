using System.Collections.Generic;
using System.Linq;
using Dalamud.Game;
using Dalamud.Game.ClientState.Party;
using Dalamud.Logging;

namespace PushyFinder.Util;

public static class PartyListSystem
{
    public delegate void PartyMemberJoinDelegate(PartyMember m);
    public delegate void PartyMemberLeaveDelegate(PartyMember m);

    public static event PartyMemberJoinDelegate OnJoin;
    public static event PartyMemberLeaveDelegate OnLeave;

    public static void Start()
    {
        PluginLog.Information("Start");
        Service.Framework.Update += Update;
    }

    public static void Stop()
    {
        PluginLog.Information("Stop");
        Service.Framework.Update -= Update;
    }

    private static List<PartyMember> members = new();
    private static List<PartyMember> oldMembers = new();

    static bool ListContainsMember(List<PartyMember> l, PartyMember m)
    {
        // oh this is incredibly fucking stupid
        foreach (var a in l)
        {
            if (a.Name.ToString() == m.Name.ToString())
                return true;
        }

        return false;
    }

    static void Update(Framework framework)
    {
        if (!Service.ClientState.IsLoggedIn)
            return;
        
        // Performance nightmare? Bad idea? Who knows. ~L
        // If we run out of RAM after running this plugin, you know how we went.
        members.Clear();
        for (var i = 0; i < Service.PartyList.Length; i++)
        {
            var addr = Service.PartyList.GetPartyMemberAddress(i);
            var mem = Service.PartyList.CreatePartyMemberReference(addr);
            if (mem != null)
                members.Add(mem);
        }
        
        if (members.Count != oldMembers.Count)
        {
            // a change has been detected
            
            // Check for joins
            foreach (var i in members)
            {
                if (!ListContainsMember(oldMembers, i))
                {
                    // member joined
                    OnJoin?.Invoke(i);
                }
            }
            
            // Check for leaves
            // Is this what we call 'iterating too much?'
            foreach (var i in oldMembers)
            {
                if (!ListContainsMember(members, i))
                {
                    // member left
                    OnLeave?.Invoke(i);
                }
            }
        }
        
        // REFERENCE FUNNIES?
        oldMembers = members.ToList();
    }
}
