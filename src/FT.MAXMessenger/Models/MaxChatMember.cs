using System.Collections.Generic;

namespace FT.MAXMessenger
{
    public class MaxChatMember : MaxUser
    {
        public string Alias { get; set; }

        public bool? IsAdmin { get; set; }

        public bool? IsOwner { get; set; }

        public long? JoinTime { get; set; }

        public long? LastAccessTime { get; set; }

        public IList<string> Permissions { get; set; }
    }
}
