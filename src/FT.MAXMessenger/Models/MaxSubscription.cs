using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace FT.MAXMessenger
{
    public class MaxSubscription
    {
        public string Url { get; set; }

        public IList<string> UpdateTypes { get; set; }

        public string Secret { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }
}
