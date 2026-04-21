using System.Collections.Generic;

namespace FT.MAXMessenger
{
    public class MaxUpdatesQuery
    {
        public int? Limit { get; set; }

        public int? Timeout { get; set; }

        public long? Marker { get; set; }

        public IList<string> Types { get; set; }
    }
}
