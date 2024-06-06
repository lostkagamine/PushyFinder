using System.Collections.Generic;
using System.Linq;
using Dalamud.Memory;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI.Info;

namespace PushyFinder.Util;

public static class CrossWorldPartyListSystem
{
    // Yes, there's already a type in Dalamud for this.
    // TODO? add more if we end up needing it
    public struct CrossWorldMember
    {
        public string Name;
        public int PartyCount;
        public uint Level;
        public uint JobId;
    }
    
    public delegate void CrossWorldJoinDelegate(CrossWorldMember m);
    public delegate void CrossWorldLeaveDelegate(CrossWorldMember m);

    public static event CrossWorldJoinDelegate? OnJoin;
    public static event CrossWorldLeaveDelegate? OnLeave;

    public static void Start()
    {
        Service.Framework.Update += Update;
    }

    public static void Stop()
    {
        Service.Framework.Update -= Update;
    }

    private static List<CrossWorldMember> members = new();
    private static List<CrossWorldMember> oldMembers = new();

    static bool ListContainsMember(List<CrossWorldMember> l, CrossWorldMember m)
    {
        // oh this is incredibly fucking stupid
        foreach (var a in l)
        {
            if (a.Name == m.Name)
                return true;
        }

        return false;
    }

    static unsafe void Update(IFramework framework)
    {
        if (!Service.ClientState.IsLoggedIn)
            return;

        if (!InfoProxyCrossRealm.IsCrossRealmParty())
            return;
        
        members.Clear();
        var partyCount = InfoProxyCrossRealm.GetPartyMemberCount();
        for (var i = 0u; i < partyCount; i++)
        {
            var addr = InfoProxyCrossRealm.GetGroupMember(i);
            var name = MemoryHelper.ReadStringNullTerminated((nint)addr->Name);
            var mObj = new CrossWorldMember
            {
                Name = name,
                PartyCount = partyCount,
                Level = addr->Level,
                JobId = addr->ClassJobId,
            };
            members.Add(mObj);
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
