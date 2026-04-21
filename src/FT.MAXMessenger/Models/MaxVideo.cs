using Newtonsoft.Json.Linq;

namespace FT.MAXMessenger
{
    public class MaxVideo
    {
        public string Token { get; set; }

        public JToken Urls { get; set; }

        public JToken Thumbnail { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public int? Duration { get; set; }
    }
}
