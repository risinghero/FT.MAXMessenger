using System.Collections.Generic;

namespace FT.MAXMessenger
{
    public class MaxChatMembersResponse
    {
        public IList<MaxChatMember> Members { get; set; }

        public long? Marker { get; set; }
    }
}
