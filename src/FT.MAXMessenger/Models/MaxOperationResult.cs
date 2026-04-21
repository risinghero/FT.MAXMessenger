using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace FT.MAXMessenger
{
    public class MaxOperationResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public IList<long> FailedUserIds { get; set; }

        public JToken FailedUserDetails { get; set; }
    }
}
