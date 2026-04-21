using System.Collections.Generic;

namespace FT.MAXMessenger
{
    public class MaxMessagesQuery
    {
        public string? ChatId { get; set; }

        public IList<string> MessageIds { get; set; }

        public long? From { get; set; }

        public long? To { get; set; }

        public int? Count { get; set; }
    }
}
