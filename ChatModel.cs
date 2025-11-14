namespace Chat_examination;

public class ChatModel
{

    public required string Username { get; set; }
    public required string Message { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public required string Reciever { get; set; }
    public MessageType Type { get; set; }


}

public enum MessageType
{
    Global,
    Private,
    Room
}