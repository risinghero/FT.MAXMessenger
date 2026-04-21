using System.Collections.Generic;

namespace FT.MAXMessenger
{
    public class MaxChatsResponse
    {
        public IList<MaxChat> Chats { get; set; }

        public long? Marker { get; set; }
    }
}
