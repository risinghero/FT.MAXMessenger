using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace FT.MAXMessenger
{
    public class MaxUpdate
    {
        public string UpdateType { get; set; }

        public long Timestamp { get; set; }

        public MaxMessage Message { get; set; }

        public string UserLocale { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }
}
