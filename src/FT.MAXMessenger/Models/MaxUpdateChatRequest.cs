namespace FT.MAXMessenger
{
    public class MaxUpdateChatRequest
    {
        public MaxChatIcon Icon { get; set; }

        public string Title { get; set; }

        public bool? Notify { get; set; }

        public string Pin { get; set; }
    }
}
