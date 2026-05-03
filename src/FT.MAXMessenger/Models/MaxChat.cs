using Newtonsoft.Json.Linq;

namespace FT.MAXMessenger
{
    public class MaxChat
    {
        public string ChatId { get; set; }

        public long? ChatMessageId { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public MaxChatIcon Icon { get; set; }

        public bool? IsPublic { get; set; }

        public long? LastEventTime { get; set; }

        public string Link { get; set; }

        public long? OwnerId { get; set; }

        public int? ParticipantsCount { get; set; }

        public JToken Participants { get; set; }

        public MaxMessage PinnedMessage { get; set; }

        public MaxUser DialogWithUser { get; set; }
    }
}
