using System.Collections.Generic;

namespace FT.MAXMessenger
{
    public class MaxChatMembersQuery
    {
        public int? Count { get; set; }

        public long? Marker { get; set; }

        public IList<long> UserIds { get; set; }
    }
}
