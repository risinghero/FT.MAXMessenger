using System.Collections.Generic;

namespace FT.MAXMessenger
{
    public class MaxCreateSubscriptionRequest
    {
        public string Url { get; set; }

        public IList<string> UpdateTypes { get; set; }

        public string Secret { get; set; }
    }
}
