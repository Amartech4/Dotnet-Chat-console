namespace Chat_examination
{
    public class ChatMessage
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string Room { get; set; }
        public string TargetUser { get; set; }
        public MessageType Type { get; set; }

    }

    public enum MessageType
    {
        Global,
        Private,
        Room
    }
}
