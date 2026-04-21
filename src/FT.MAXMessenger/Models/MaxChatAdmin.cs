using System.Collections.Generic;

namespace FT.MAXMessenger
{
    public class MaxChatAdmin
    {
        public long UserId { get; set; }

        public IList<string> Permissions { get; set; }

        public string Alias { get; set; }
    }
}
