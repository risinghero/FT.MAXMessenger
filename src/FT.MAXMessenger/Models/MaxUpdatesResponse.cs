using System.Collections.Generic;

namespace FT.MAXMessenger
{
    public class MaxUpdatesResponse
    {
        public IList<MaxUpdate> Updates { get; set; }

        public long? Marker { get; set; }
    }
}
